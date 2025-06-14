using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int health = 6;
    public float movementSpeed = 10f;

    [Header("Weapon")]
    public int weaponDamage = 1;
    public float weaponSwingRechargeTime = 0.35f;
    public float weaponAttackRange = 2f;
    public float weaponAttackArc = 45f;
}
