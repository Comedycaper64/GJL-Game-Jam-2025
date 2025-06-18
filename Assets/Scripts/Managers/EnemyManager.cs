using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static int meleeEnemySpawnIndex = 0;
    private static int rangedEnemySpawnIndex = 0;

    [SerializeField]
    private MeleeEnemyStateMachine[] localMeleeEnemies;

    [SerializeField]
    private RangedEnemyStateMachine[] localRangedEnemies;

    private static MeleeEnemyStateMachine[] meleeEnemies;
    private static RangedEnemyStateMachine[] rangedEnemies;

    private void OnEnable()
    {
        meleeEnemySpawnIndex = 0;
        rangedEnemySpawnIndex = 0;

        meleeEnemies = localMeleeEnemies;
        rangedEnemies = localRangedEnemies;

        PlayerManager.OnPlayerDead += ResetEnemies;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerDead -= ResetEnemies;
    }

    public static void SpawnEnemy(bool meleeEnemy, Vector2 spawnLocation)
    {
        StateMachine enemyToSpawn;
        if (meleeEnemy)
        {
            enemyToSpawn = meleeEnemies[meleeEnemySpawnIndex];
            meleeEnemySpawnIndex++;

            if (meleeEnemySpawnIndex >= meleeEnemies.Length)
            {
                meleeEnemySpawnIndex = 0;
            }
        }
        else
        {
            enemyToSpawn = rangedEnemies[rangedEnemySpawnIndex];
            rangedEnemySpawnIndex++;

            if (rangedEnemySpawnIndex >= rangedEnemies.Length)
            {
                rangedEnemySpawnIndex = 0;
            }
        }

        enemyToSpawn.transform.position = spawnLocation;
        enemyToSpawn.SpawnEnemy();
    }

    private void ResetEnemies(object sender, bool playerDead)
    {
        if (!playerDead)
        {
            foreach (StateMachine sm in meleeEnemies)
            {
                sm.ResetEnemy();
            }
            foreach (StateMachine sm in rangedEnemies)
            {
                sm.ResetEnemy();
            }
        }
    }
}
