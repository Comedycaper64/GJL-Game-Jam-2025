using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool projectileActive = false;

    private int damage;
    private float speed;
    private Vector3 direction;

    [SerializeField]
    private Collider2D projectileCollider;

    [SerializeField]
    private GameObject projectileVisual;

    private void Awake()
    {
        ToggleProjectile(false);
    }

    private void Update()
    {
        if (!projectileActive)
        {
            return;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void ToggleProjectile(bool toggle)
    {
        projectileVisual.SetActive(toggle);
        projectileCollider.enabled = toggle;
        projectileActive = toggle;
    }

    public void Spawn(Vector2 flightDirection, int projectileDamage, float projectileSpeed)
    {
        direction = flightDirection;
        damage = projectileDamage;
        speed = projectileSpeed;

        ToggleProjectile(true);
    }

    public void Deactivate()
    {
        ToggleProjectile(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Collider Entered");
        if (other.TryGetComponent<PlayerAbsorber>(out PlayerAbsorber absorber))
        {
            absorber.AbsorbProjectile();
            Deactivate();
        }
        else if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && healthSystem.GetIsPlayer()
        )
        {
            //Debug.Log("Player Damaged");

            healthSystem.TakeDamage(damage);
            Deactivate();
        }
    }
}
