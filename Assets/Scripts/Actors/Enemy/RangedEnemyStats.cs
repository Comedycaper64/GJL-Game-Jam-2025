using UnityEngine;

public class RangedEnemyStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int health = 3;
    public float movementSpeed = 4f;
    public float retreatRange = 3f;

    [Header("Weapon")]
    public int weaponDamage = 1;
    public int defaultDamage = 1;
    public int hardDamage = 2;
    public float projectileSpeed = 4f;
    public float defaultSpeed = 4f;
    public float hardSpeed = 4f;
    public float easySpeed = 4f;
    public float weaponAttackRange = 6f;
    public float weaponAttackInterval = 2.5f;
    public float defaultAttackInterval = 2.5f;
    public float easyAttackInterval = 2.5f;
    public float hardAttackInterval = 2.5f;
    public float weaponAttackVariance = .75f;
    public float weaponAttackTiming = 1f;
    public float weaponAttackSpread = .5f;

    private void Start()
    {
        //If feedback on difficulty has been given, modify enemy stats
        if (FeedbackManager.Instance.TryGetDictionaryValue("Diff", out int val))
        {
            if (val == 1)
            {
                weaponDamage = hardDamage;
                projectileSpeed = hardSpeed;
                weaponAttackInterval = hardAttackInterval;
            }
            else if (val == 2)
            {
                weaponDamage = defaultDamage;
                projectileSpeed = easySpeed;
                weaponAttackInterval = easyAttackInterval;
            }
            else
            {
                weaponDamage = defaultDamage;
                projectileSpeed = defaultSpeed;
                weaponAttackInterval = defaultAttackInterval;
            }
        }
    }
}
