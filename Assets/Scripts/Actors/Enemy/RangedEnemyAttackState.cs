using UnityEngine;

public class RangedEnemyAttackState : State
{
    private bool hasAttacked;
    float stateTime = 1.5f;
    float stateTimer;
    private RangedEnemyStats stats;
    private RangedEnemyStateMachine enemyStateMachine;
    private HealthSystem playerHealth;

    public RangedEnemyAttackState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as RangedEnemyStateMachine;
        playerHealth = enemyStateMachine.GetPlayerTransform().GetComponent<HealthSystem>();
    }

    public override void Enter()
    {
        stats = enemyStateMachine.GetStats();
        //stateMachine.smAnimator.SetTrigger("attack");
        hasAttacked = false;
        stateTimer = stateTime;
    }

    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        stateTimer -= deltaTime;

        if (!hasAttacked && stateTimer <= (stateTime * (1 - stats.weaponAttackTiming)))
        {
            ResolveAttack();

            hasAttacked = true;
        }

        if (stateTimer <= 0f)
        {
            stateMachine.SwitchState(new RangedEnemyAimState(stateMachine));
            return;
        }
    }

    private void ResolveAttack()
    {
        float randomSpread = Random.Range(-stats.weaponAttackSpread, stats.weaponAttackSpread);

        Vector2 playerDirectionFromEnemy = (
            playerHealth.transform.position
            + new Vector3(randomSpread, randomSpread)
            - stateMachine.transform.position
        ).normalized;

        ProjectileManager.SpawnProjectile(
            stateMachine.transform.position,
            playerDirectionFromEnemy,
            stats.weaponDamage,
            stats.projectileSpeed
        );
    }
}
