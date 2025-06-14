using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private bool weaponActive = false;
    private bool weaponAvailable = false;
    private float weaponRechargeTimer = 0f;
    private float weaponRechargeTime;

    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private PlayerCursorPointer cursorPointer;

    [SerializeField]
    private LayerMask hittableLayerMask;

    private void Start()
    {
        weaponRechargeTime = playerStats.weaponSwingRechargeTime;

        ToggleWeapon(true);
    }

    private void OnDisable()
    {
        InputManager.OnAttackEvent -= TryAttack;
    }

    private void Update()
    {
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

    private void TryAttack()
    {
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            playerStats.weaponAttackRange,
            hittableLayerMask
        );

        Vector2 cursorDirection = cursorPointer.GetCursorDirection();

        foreach (Collider2D collider in colliders)
        {
            if (!collider.TryGetComponent<HealthSystem>(out HealthSystem healthSystem))
            {
                continue;
            }

            if (healthSystem.GetIsPlayer())
            {
                return;
            }

            Vector2 colliderDirectionFromPlayer = (
                collider.transform.position - transform.position
            ).normalized;
            if (
                Vector2.Dot(cursorDirection, colliderDirectionFromPlayer)
                > (1f - playerStats.weaponAttackArc)
            )
            {
                healthSystem.TakeDamage(playerStats.weaponDamage);
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
}
