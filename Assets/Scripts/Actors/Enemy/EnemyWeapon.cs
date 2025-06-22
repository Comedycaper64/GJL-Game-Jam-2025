using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private bool sfxVariance = false;
    private float damp = 5f;

    private Vector2 playerTarget = Vector2.up;

    [SerializeField]
    private float sfxVolume = 0.25f;

    [SerializeField]
    private Transform weaponHolderTransform;

    [SerializeField]
    private SpriteRenderer weaponSprite;

    [SerializeField]
    private Animator enemyWeaponAnim;

    [SerializeField]
    private AudioClip weaponSwingSFX;

    [SerializeField]
    private AudioClip weaponSwingAltSFX;

    private void Start()
    {
        //If feedback on sound effects has been given, modify sound effect
        if (FeedbackManager.Instance.TryGetDictionaryValue("SFX", out int val))
        {
            if (val == 1)
            {
                sfxVariance = true;
                weaponSwingSFX = weaponSwingAltSFX;
            }
            else if (val == 2)
            {
                sfxVolume = 0f;
            }
        }
    }

    private void Update()
    {
        RotateTowardsTargetDirection();
    }

    private void RotateTowardsTargetDirection()
    {
        Quaternion rotationAngle = Quaternion.LookRotation(
            (Vector3)playerTarget - transform.position,
            Vector3.forward
        );
        weaponHolderTransform.rotation = Quaternion.Slerp(
            weaponHolderTransform.rotation,
            rotationAngle,
            Time.deltaTime * damp
        );
        weaponHolderTransform.eulerAngles = new Vector3(0, 0, weaponHolderTransform.eulerAngles.z);
    }

    public void PlayAttackAnimation()
    {
        enemyWeaponAnim.SetTrigger("attack");
        AudioManager.PlaySFX(weaponSwingSFX, sfxVolume, 5, transform.position, sfxVariance);
    }

    public void ToggleWeaponVisual(bool toggle)
    {
        weaponSprite.enabled = toggle;
    }

    public void SetAttackDirection(Vector2 direction)
    {
        playerTarget = direction;
    }
}
