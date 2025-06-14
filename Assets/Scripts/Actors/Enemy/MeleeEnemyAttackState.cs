using UnityEngine;

public class MeleeEnemyAttackState : State
{
    private bool hasAttacked;
    float stateTime = 1.5f;
    float stateTimer;
    private MeleeEnemyStats stats;
    private MeleeEnemyStateMachine enemyStateMachine;
    private HealthSystem playerHealth;

    public MeleeEnemyAttackState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as MeleeEnemyStateMachine;
        playerHealth = enemyStateMachine.GetPlayerTransform().GetComponent<HealthSystem>();
    }

    public override void Enter()
    {
        stats = enemyStateMachine.GetStats();
        stateMachine.smAnimator.SetTrigger("attack");
        hasAttacked = false;
        stateTimer = stateTime;
    }

    public override void Exit() { }

    public override void Tick(float deltaTime)
    {
        stateTimer -= deltaTime;

        if (!hasAttacked && stateTimer <= (stateTime * (1 - stats.weaponAttackTiming)))
        {
            ResolveAttack(enemyStateMachine.GetAttackDirection());

            // if (
            //     Vector2.Distance(playerHealth.transform.position, stateMachine.transform.position)
            //     < stats.weaponDamageRange
            // )
            // {
            //     playerHealth.TakeDamage(stats.weaponDamage);
            // }


            hasAttacked = true;
        }

        if (stateTimer <= 0f)
        {
            stateMachine.SwitchState(new MeleeEnemyApproachState(stateMachine));
            return;
        }
    }

    private void ResolveAttack(Vector2 attackDirection)
    {
        Vector2 playerDirectionFromEnemy = (
            playerHealth.transform.position - stateMachine.transform.position
        ).normalized;

        if (Vector2.Dot(attackDirection, playerDirectionFromEnemy) > (1f - stats.weaponAttackArc))
        {
            playerHealth.TakeDamage(stats.weaponDamage);
        }
    }
}
