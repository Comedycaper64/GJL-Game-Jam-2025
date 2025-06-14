using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int health = 6;
    public float movementSpeed = 10f;
    public float dashSpeedModifier = 5f;
    public float dashTime = 0.15f;
    public float dashRechargeTime = 1f;

    [Header("Weapon")]
    public int weaponDamage = 1;
    public float weaponSwingRechargeTime = 0.35f;
    public float weaponAttackRange = 2f;

    [Range(0, 1)]
    public float weaponAttackArc = 0.5f;
}
