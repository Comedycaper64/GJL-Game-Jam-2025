using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool playerProjectile = false;
    private bool projectileActive = false;

    private int damage;
    private float speed;
    private Vector3 direction;

    [SerializeField]
    private Collider2D projectileCollider;

    [SerializeField]
    private Color enemyColour;

    [SerializeField]
    private Color playerColour;

    [SerializeField]
    private SpriteRenderer projectileRenderer;

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

    public void Spawn(
        Vector2 flightDirection,
        int projectileDamage,
        float projectileSpeed,
        bool playerProjectile = false
    )
    {
        direction = flightDirection;
        damage = projectileDamage;
        speed = projectileSpeed;

        projectileRenderer.color = enemyColour;

        this.playerProjectile = playerProjectile;

        if (playerProjectile)
        {
            projectileRenderer.color = playerColour;
        }

        ToggleProjectile(true);
    }

    public void Deactivate()
    {
        ToggleProjectile(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerProjectile)
        {
            if (
                other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
                && !healthSystem.GetIsPlayer()
            )
            {
                healthSystem.TakeDamage(damage);
                Deactivate();
            }
            return;
        }

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
