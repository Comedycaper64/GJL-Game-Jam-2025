using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private float damp = 5f;

    private Vector2 playerTarget = Vector2.up;

    [SerializeField]
    private Transform weaponHolderTransform;

    [SerializeField]
    private SpriteRenderer weaponSprite;

    [SerializeField]
    private Animator enemyWeaponAnim;

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
    }

    public void ToggleWeaponVisual(bool toggle)
    {
        weaponSprite.enabled = toggle;
    }

    public void SetAttackDirection(Vector2 direction)
    {
        playerTarget = direction;

        Debug.Log("Target direction: " + playerTarget);
    }
}
