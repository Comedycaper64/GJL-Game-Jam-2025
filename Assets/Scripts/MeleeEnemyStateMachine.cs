using System;
using UnityEngine;

public class MeleeEnemyStateMachine : MonoBehaviour
{
    private Transform playerTransform;
    private HealthSystem health;
    private MeleeEnemyStats stats;

    [SerializeField]
    private Collider2D bodyCollider;

    private void Awake()
    {
        health = GetComponent<HealthSystem>();
        stats = GetComponent<MeleeEnemyStats>();

        bodyCollider.enabled = false;
        health.SetMaxHealth(stats.health);
        health.OnTakeDamage += HealthSystem_OnTakeDamage;
        health.OnDeath += HealthSystem_OnDeath;
    }

    private void Start()
    {
        playerTransform = PlayerIdentifier.PlayerTransform;
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
        //Switch to dead state
    }
}
