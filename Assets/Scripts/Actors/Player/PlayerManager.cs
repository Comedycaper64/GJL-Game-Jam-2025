using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private HealthSystem playerHealth;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerHealth = GetComponent<HealthSystem>();
        playerStats = GetComponent<PlayerStats>();

        playerHealth.SetMaxHealth(playerStats.health);
    }
}
