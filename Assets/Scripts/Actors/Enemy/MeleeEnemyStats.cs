using UnityEngine;

public class MeleeEnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int health = 3;
    public float movementSpeed = 4f;

    [Header("Weapon")]
    public int weaponDamage = 1;
    public float weaponAttackRange = 2f;
    public float weaponDamageRange = 2.5f;
    public float weaponAttackInterval = 2.5f;
    public float weaponAttackVariance = .75f;
    public float weaponAttackTiming = 1f;

    [Range(0, 1)]
    public float weaponAttackArc = .5f;
}
