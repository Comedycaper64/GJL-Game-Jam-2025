using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private bool playerDead = false;
    private HealthSystem playerHealth;
    private PlayerStats playerStats;
    private Transform lastCheckpointLocation;

    public static EventHandler<bool> OnPlayerDead;

    private void Awake()
    {
        playerHealth = GetComponent<HealthSystem>();
        playerStats = GetComponent<PlayerStats>();

        playerHealth.SetMaxHealth(playerStats.health);

        InputManager.OnRespawnEvent += RespawnPlayer;
        playerHealth.OnDeath += KillPlayer;
    }

    private void OnDisable()
    {
        InputManager.OnRespawnEvent -= RespawnPlayer;
        playerHealth.OnDeath -= KillPlayer;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerCheckpoint>(out PlayerCheckpoint playerCheckpoint))
        {
            lastCheckpointLocation = playerCheckpoint.GetCheckPoint();
        }
    }

    private void KillPlayer(object sender, EventArgs eventArgs)
    {
        if (playerDead)
        {
            return;
        }
        playerDead = true;

        OnPlayerDead?.Invoke(this, true);
    }

    private void RespawnPlayer()
    {
        if (!playerDead)
        {
            return;
        }

        playerDead = false;
        playerHealth.Heal(999);
        transform.position = lastCheckpointLocation.position;
        OnPlayerDead?.Invoke(this, false);
    }
}
