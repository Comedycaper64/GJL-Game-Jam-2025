using UnityEngine;

public class RangedEnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int health = 3;
    public float movementSpeed = 4f;
    public float retreatRange = 3f;

    [Header("Weapon")]
    public int weaponDamage = 1;
    public float projectileSpeed = 4f;
    public float weaponAttackRange = 6f;
    public float weaponAttackInterval = 2.5f;
    public float weaponAttackVariance = .75f;
    public float weaponAttackTiming = 1f;
    public float weaponAttackSpread = .5f;
}
