using UnityEngine;

public class RangedEnemyAimState : State
{
    private float weaponAttackTimer = 0f;
    private float weaponAttackTime;
    private RangedEnemyStats stats;
    private RangedEnemyStateMachine enemyStateMachine;

    public RangedEnemyAimState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as RangedEnemyStateMachine;
    }

    public override void Enter()
    {
        stats = enemyStateMachine.GetStats();
        weaponAttackTime =
            stats.weaponAttackInterval
            + Random.Range(-stats.weaponAttackVariance, stats.weaponAttackVariance);
        weaponAttackTimer = 0f;

        //stateMachine.smAnimator.SetBool("chasing", true);
    }

    public override void Exit()
    {
        //stateMachine.smAnimator.SetBool("chasing", false);

        Vector2 attackDirection = (
            enemyStateMachine.GetPlayerTransform().position - stateMachine.transform.position
        ).normalized;
    }

    public override void Tick(float deltaTime)
    {
        // if (!stateMachine.canMove)
        // {
        //     return;
        // }

        float distanceToPlayer = Vector2.Distance(
            enemyStateMachine.GetPlayerTransform().position,
            stateMachine.transform.position
        );

        Vector3 directionToMove = Vector3.zero;

        if (distanceToPlayer > stats.weaponAttackRange)
        {
            directionToMove = (
                enemyStateMachine.GetPlayerTransform().position - stateMachine.transform.position
            ).normalized;
            directionToMove.z = 0f;
        }
        else if (distanceToPlayer < stats.retreatRange)
        {
            directionToMove = -(
                enemyStateMachine.GetPlayerTransform().position - stateMachine.transform.position
            ).normalized;
            directionToMove.z = 0f;
        }
        else
        {
            weaponAttackTimer += deltaTime;

            if (weaponAttackTimer > weaponAttackTime)
            {
                stateMachine.SwitchState(new RangedEnemyAttackState(stateMachine));
                return;
            }
        }

        stateMachine.transform.position += directionToMove * stats.movementSpeed * deltaTime;
    }
}
