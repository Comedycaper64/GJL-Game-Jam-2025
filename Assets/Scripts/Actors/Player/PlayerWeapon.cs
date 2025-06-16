using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private bool playerDead = false;
    private bool weaponActive = false;
    private bool weaponAvailable = false;
    private float weaponRechargeTimer = 0f;
    private float weaponRechargeTime;

    private List<HealthSystem> enemiesInRange = new List<HealthSystem>();

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private PlayerCursorPointer cursorPointer;

    [SerializeField]
    private CircleCollider2D weaponCollider;

    // [SerializeField]
    // private LayerMask hittableLayerMask;

    private void Start()
    {
        weaponRechargeTime = playerStats.weaponSwingRechargeTime;

        ToggleWeapon(true);

        PlayerManager.OnPlayerDead += DisableWeapon;
        weaponCollider.radius = playerStats.weaponAttackRange;
    }

    private void OnDisable()
    {
        InputManager.OnAttackEvent -= TryAttack;
        PlayerManager.OnPlayerDead -= DisableWeapon;
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

        ResolveAttack();

        //Play Weapon Effect
        //Trigger animation
        //Play SFX

        weaponAvailable = false;
    }

    private void ResolveAttack()
    {
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
                health.TakeDamage(playerStats.weaponDamage);
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
