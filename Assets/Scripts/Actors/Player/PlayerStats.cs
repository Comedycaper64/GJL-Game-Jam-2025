using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int health = 6;
    public float movementSpeed = 10f;
    public float defaultSpeed = 10f;
    public float slowSpeed = 10f;
    public float fastSpeed = 10f;
    public float dashSpeedModifier = 5f;
    public float dashTime = 0.15f;
    public float dashRechargeTime = 1f;

    [Header("Weapon")]
    public int weaponDamage = 1;
    public int defaultDamage = 1;
    public int damageBuff = 0;
    public int highDamage = 1;
    public float weaponSwingRechargeTime = 0.35f;
    public float weaponAttackRange = 2f;

    [Range(0, 1)]
    public float weaponAttackArc = 0.5f;
    public float weaponKnockbackStrength = 10f;

    [Header("Absorb")]
    public float meterReplenishRate = 0.2f;
    public float meterDrainRate = 0.5f;

    private void Start()
    {
        //If feedback on movement speed has been given, modify movement speed
        if (FeedbackManager.Instance.TryGetDictionaryValue("Speed", out int val))
        {
            if (val == 1)
            {
                movementSpeed = fastSpeed;
            }
            else if (val == 2)
            {
                movementSpeed = slowSpeed;
            }
            else
            {
                movementSpeed = defaultSpeed;
            }
        }

        //If feedback on attacking has been given, modify damage
        if (FeedbackManager.Instance.TryGetDictionaryValue("Attack", out int val2))
        {
            if (val2 == 2)
            {
                weaponDamage = highDamage;
            }
            else
            {
                weaponDamage = defaultDamage;
            }
        }
    }

    public int GetWeaponDamage()
    {
        return weaponDamage + damageBuff;
    }
}
