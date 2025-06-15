using System;
using UnityEngine;

public class RangedEnemyStateMachine : StateMachine
{
    private Transform playerTransform;
    private HealthSystem health;
    private RangedEnemyStats stats;

    [SerializeField]
    private Collider2D bodyCollider;

    [SerializeField]
    private GameObject enemyVisual;

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        stats = GetComponent<RangedEnemyStats>();

        ToggleCollider(false);
        health.SetMaxHealth(stats.health);
        health.OnTakeDamage += HealthSystem_OnTakeDamage;
        health.OnDeath += HealthSystem_OnDeath;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;

        //TEMP
        SwitchState(new RangedEnemySpawnState(this));
    }

    private void OnDisable()
    {
        health.OnTakeDamage -= HealthSystem_OnTakeDamage;
        health.OnDeath -= HealthSystem_OnDeath;
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
        SwitchState(new RangedEnemyDeadState(this));
    }

    public RangedEnemyStats GetStats()
    {
        return stats;
    }

    public HealthSystem GetHealthSystem()
    {
        return health;
    }

    public void ToggleCollider(bool toggle)
    {
        bodyCollider.enabled = toggle;
    }

    public override void ToggleInactive(bool toggle)
    {
        ToggleCollider(!toggle);
        enemyVisual.SetActive(!toggle);
    }
}
