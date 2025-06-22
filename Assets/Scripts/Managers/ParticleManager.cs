using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    private static int particleIndex = 0;

    [SerializeField]
    private ParticleObject[] localParticles;
    private static ParticleObject[] particles;

    private void OnEnable()
    {
        particles = localParticles;

        particleIndex = 0;
    }

    public static void SpawnParticles(Vector3 spawnPosition, Vector2 impactDirection)
    {
        ParticleObject spawnedParticle = particles[particleIndex];

        spawnedParticle.transform.position = spawnPosition;

        float rotationAngle = Mathf.Atan2(impactDirection.y, impactDirection.x) * Mathf.Rad2Deg;
        spawnedParticle.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, rotationAngle));

        spawnedParticle.PlayParticleEffect();

        particleIndex++;

        if (particleIndex >= particles.Length)
        {
            particleIndex = 0;
        }
    }
}
