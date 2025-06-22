using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private bool sfxVariance = false;
    private bool playerDead = false;
    private bool weaponActive = false;
    private bool weaponAvailable = false;
    private bool weaponFeedback = false;

    [SerializeField]
    private bool knockbackEnemies = true;
    private float weaponRechargeTimer = 0f;
    private float weaponRechargeTime;
    private float weaponSwingDamageDelay = 0.2f;

    [SerializeField]
    private float sfxVolume = 0.25f;

    private Coroutine weaponSwingCoroutine;

    private List<HealthSystem> enemiesInRange = new List<HealthSystem>();

    [SerializeField]
    private Animator weaponAnimator;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private PlayerCursorPointer cursorPointer;

    [SerializeField]
    private CircleCollider2D weaponCollider;

    [SerializeField]
    private AudioClip weaponSwingSFX;

    [SerializeField]
    private AudioClip altWeaponSwingSFX;

    private void Start()
    {
        weaponRechargeTime = playerStats.weaponSwingRechargeTime;

        ToggleWeapon(true);

        PlayerManager.OnPlayerDead += DisableWeapon;
        weaponCollider.radius = playerStats.weaponAttackRange;

        //If feedback on sound effects has been given, modify sound effect
        if (FeedbackManager.Instance.TryGetDictionaryValue("SFX", out int val))
        {
            if (val == 1)
            {
                sfxVariance = true;
                weaponSwingSFX = altWeaponSwingSFX;
            }
            else if (val == 2)
            {
                sfxVolume = 0f;
            }
        }

        //If feedback on attacking has been given, modify weapon feedbacks
        if (FeedbackManager.Instance.TryGetDictionaryValue("Attack", out int val2))
        {
            if (val2 == 1)
            {
                weaponFeedback = true;
            }
        }
    }

    private void OnDisable()
    {
        InputManager.OnAttackEvent -= TryAttack;
        PlayerManager.OnPlayerDead -= DisableWeapon;

        if (weaponSwingCoroutine != null)
        {
            StopCoroutine(weaponSwingCoroutine);
        }
    }

    private void Update()
    {
        if (playerDead)
        {
            return;
        }

        if (!weaponAvailable)
        {
            weaponRechargeTimer += Time.deltaTime;

            if (weaponRechargeTimer >= weaponRechargeTime)
            {
                weaponAvailable = true;
                weaponRechargeTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && !healthSystem.GetIsPlayer()
        )
        {
            enemiesInRange.Add(healthSystem);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && enemiesInRange.Contains(healthSystem)
        )
        {
            enemiesInRange.Remove(healthSystem);
        }
    }

    private void TryAttack()
    {
        if (playerDead)
        {
            return;
        }

        if (!weaponAvailable)
        {
            return;
        }

        weaponAnimator.SetTrigger("slash");

        AudioManager.PlaySFX(weaponSwingSFX, sfxVolume, 0, transform.position, sfxVariance);

        weaponAvailable = false;

        weaponSwingCoroutine = StartCoroutine(AttackDamageDelay());
    }

    private IEnumerator AttackDamageDelay()
    {
        yield return new WaitForSeconds(weaponSwingDamageDelay);

        ResolveAttack();
    }

    private void ResolveAttack()
    {
        bool damageDealtOnce = false;

        Vector2 cursorDirection = cursorPointer.GetCursorDirection();

        for (int i = 0; i < enemiesInRange.Count; i++)
        {
            HealthSystem health = enemiesInRange[i];

            Vector2 colliderDirectionFromPlayer = (
                health.transform.position - transform.position
            ).normalized;

            if (
                Vector2.Dot(cursorDirection, colliderDirectionFromPlayer)
                > (1f - playerStats.weaponAttackArc)
            )
            {
                DealDamage(health, colliderDirectionFromPlayer);

                if (!damageDealtOnce)
                {
                    health.PlayDamagedSound();
                    damageDealtOnce = true;
                }
            }
        }
    }

    private void DealDamage(HealthSystem health, Vector2 attackDirection)
    {
        health.TakeDamage(playerStats.GetWeaponDamage());

        if (weaponFeedback)
        {
            ParticleManager.SpawnParticles(health.transform.position, attackDirection);
        }

        if (knockbackEnemies)
        {
            if (health.TryGetComponent<Rigidbody2D>(out Rigidbody2D damagedRb))
            {
                damagedRb.AddForce(
                    attackDirection * playerStats.weaponKnockbackStrength,
                    ForceMode2D.Force
                );
            }
        }
    }

    public void ToggleWeapon(bool toggle)
    {
        if (toggle)
        {
            if (!weaponActive)
            {
                InputManager.OnAttackEvent += TryAttack;
                weaponActive = true;
            }
        }
        else
        {
            InputManager.OnAttackEvent -= TryAttack;
            weaponActive = false;
        }
    }

    private void DisableWeapon(object sender, bool toggle)
    {
        playerDead = toggle;
        enemiesInRange.Clear();
    }
}
