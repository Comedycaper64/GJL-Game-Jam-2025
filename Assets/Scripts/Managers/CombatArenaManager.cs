using System;
using System.Collections;
using UnityEngine;

[Serializable]
struct EnemyWave
{
    public int meleeEnemies;
    public int rangedEnemies;
}

public class CombatArenaManager : MonoBehaviour
{
    private bool arenaCompleted = false;
    private bool arenaActive = false;
    private int waveIndex = 0;
    private int spawnedEnemies = 0;

    [SerializeField]
    private Collider2D combatStartArea;

    [SerializeField]
    private ArenaGate[] arenaGates;

    [SerializeField]
    private EnemyWave[] enemyWaves;

    [SerializeField]
    private Transform[] arenaSpawnPoints;

    [SerializeField]
    private DialogueCluster combatEndCluster;

    public static EventHandler<DialogueCluster> OnCombatEnd;

    private void OnEnable()
    {
        PlayerManager.OnPlayerDead += TryReset;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerDead -= TryReset;
    }

    private void BeginEncounter()
    {
        if (arenaCompleted)
        {
            return;
        }

        waveIndex = 0;
        spawnedEnemies = 0;

        combatStartArea.enabled = false;
        SpawnEnemyWave();

        arenaActive = true;
        StateMachine.OnEnemyDeath += DecrementSpawnedEnemies;
        ToggleGates(true);
    }

    private void EndEncounter()
    {
        StateMachine.OnEnemyDeath -= DecrementSpawnedEnemies;
        arenaActive = false;
        ToggleGates(false);
        arenaCompleted = true;

        if (combatEndCluster)
        {
            OnCombatEnd?.Invoke(this, combatEndCluster);
        }
    }

    private void ToggleGates(bool toggle)
    {
        foreach (ArenaGate gate in arenaGates)
        {
            gate.ToggleGate(toggle);
        }
    }

    private void DecrementSpawnedEnemies()
    {
        if (!arenaActive)
        {
            return;
        }

        spawnedEnemies--;

        if (spawnedEnemies <= 0)
        {
            SpawnEnemyWave();
        }
    }

    private void SpawnEnemyWave()
    {
        if (waveIndex >= enemyWaves.Length)
        {
            EndEncounter();
            return;
        }

        EnemyWave currentWave = enemyWaves[waveIndex];

        for (int i = 0; i < currentWave.meleeEnemies; i++)
        {
            EnemyManager.SpawnEnemy(true, arenaSpawnPoints[i].position);
        }

        for (int i = 0; i < currentWave.rangedEnemies; i++)
        {
            EnemyManager.SpawnEnemy(false, arenaSpawnPoints[i + currentWave.meleeEnemies].position);
        }

        spawnedEnemies = currentWave.meleeEnemies + currentWave.rangedEnemies;

        waveIndex++;
    }

    public void ResetArena()
    {
        if (arenaCompleted)
        {
            return;
        }

        StateMachine.OnEnemyDeath -= DecrementSpawnedEnemies;

        arenaActive = false;
        ToggleGates(false);

        StartCoroutine(DelayedArenaReset());
    }

    private IEnumerator DelayedArenaReset()
    {
        yield return new WaitForSeconds(0.1f);

        combatStartArea.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && healthSystem.GetIsPlayer()
        )
        {
            BeginEncounter();
        }
    }

    private void TryReset(object sender, bool playerDead)
    {
        if (!playerDead)
        {
            ResetArena();
        }
    }
}
