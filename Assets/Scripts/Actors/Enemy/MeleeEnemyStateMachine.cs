using System;
using UnityEngine;

public class MeleeEnemyStateMachine : StateMachine
{
    private Vector2 attackDirection;
    private Transform playerTransform;
    private HealthSystem health;
    private MeleeEnemyStats stats;

    [SerializeField]
    private Collider2D bodyCollider;

    [SerializeField]
    private Rigidbody2D enemyRB;

    [SerializeField]
    private GameObject enemyVisual;

    [SerializeField]
    private LayerMask hittableLayerMask;

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        stats = GetComponent<MeleeEnemyStats>();

        ToggleCollider(false);
        health.SetMaxHealth(stats.health);
        health.OnTakeDamage += HealthSystem_OnTakeDamage;
        health.OnDeath += HealthSystem_OnDeath;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;

        ResetEnemy();
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= HealthSystem_OnTakeDamage;
        health.OnDeath -= HealthSystem_OnDeath;
    }

    public override void SpawnEnemy()
    {
        health.SetMaxHealth(stats.health);
        SwitchState(new MeleeEnemySpawnState(this));
    }

    public Transform GetPlayerTransform()
    {
        return playerTransform;
    }

    private void HealthSystem_OnTakeDamage()
    {
        //Damage feedback
        //Play hit sound?
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        SwitchState(new MeleeEnemyDeadState(this));
        OnEnemyDeath?.Invoke();
    }

    public Vector2 GetAttackDirection()
    {
        return attackDirection;
    }

    public void SetAttackDirection(Vector2 direction)
    {
        attackDirection = direction;
    }

    public MeleeEnemyStats GetStats()
    {
        return stats;
    }

    public HealthSystem GetHealthSystem()
    {
        return health;
    }

    public LayerMask GetHittableLayerMask()
    {
        return hittableLayerMask;
    }

    public Rigidbody2D GetRigidbody()
    {
        return enemyRB;
    }

    public void ToggleCollider(bool toggle)
    {
        bodyCollider.enabled = toggle;
    }

    public void ToggleVisual(bool toggle)
    {
        enemyVisual.SetActive(toggle);
    }

    public override void ToggleInactive(bool toggle)
    {
        ToggleCollider(!toggle);
        ToggleVisual(!toggle);
    }
}
