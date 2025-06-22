using UnityEngine;

public class ParticleObject : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particlesSystem;

    public void PlayParticleEffect()
    {
        particlesSystem.Play();
    }
}
