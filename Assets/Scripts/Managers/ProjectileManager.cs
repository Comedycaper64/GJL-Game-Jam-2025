using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private static int projectileIndex = 0;

    [SerializeField]
    private Projectile[] localProjectiles;
    private static Projectile[] projectiles;

    private void OnEnable()
    {
        projectiles = localProjectiles;

        projectileIndex = 0;
    }

    public static void SpawnProjectile(
        Vector3 spawnPosition,
        Vector2 flightDirection,
        int projectileDamage,
        float projectileSpeed,
        bool playerProjectile = false
    )
    {
        Projectile spawnedProjectile = projectiles[projectileIndex];

        spawnedProjectile.transform.position = spawnPosition;

        float rotationAngle = Mathf.Atan2(flightDirection.y, flightDirection.x) * Mathf.Rad2Deg;
        spawnedProjectile.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotationAngle));

        spawnedProjectile.Spawn(
            flightDirection,
            projectileDamage,
            projectileSpeed,
            playerProjectile
        );

        projectileIndex++;

        if (projectileIndex >= projectiles.Length)
        {
            projectileIndex = 0;
        }
    }
}
