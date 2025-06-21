using UnityEngine;

public class MeleeEnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int health = 3;
    public float movementSpeed = 4f;
    public float defaultMoveSpeed = 4f;
    public float easyMoveSpeed = 4f;
    public float hardMoveSpeed = 4f;
    public float retreatRange = 1f;

    [Header("Weapon")]
    public int weaponDamage = 1;
    public int defaultDamage = 1;
    public int hardDamage = 2;
    public float weaponAttackRange = 2f;
    public float weaponDamageRange = 2.5f;
    public float weaponAttackInterval = 2.5f;
    public float defaultAttackInterval = 2.5f;
    public float easyAttackInterval = 2.5f;
    public float hardAttackInterval = 2.5f;
    public float weaponAttackVariance = .75f;
    public float weaponAttackTiming = 1f;

    [Range(0, 1)]
    public float weaponAttackArc = .5f;

    private void Start()
    {
        if (FeedbackManager.Instance.TryGetDictionaryValue("Diff", out int val))
        {
            if (val == 1)
            {
                weaponDamage = hardDamage;
                movementSpeed = hardMoveSpeed;
                weaponAttackInterval = hardAttackInterval;
            }
            else if (val == 2)
            {
                weaponDamage = defaultDamage;
                movementSpeed = easyMoveSpeed;
                weaponAttackInterval = easyAttackInterval;
            }
            else
            {
                weaponDamage = defaultDamage;
                movementSpeed = defaultMoveSpeed;
                weaponAttackInterval = defaultAttackInterval;
            }
        }
    }
}
