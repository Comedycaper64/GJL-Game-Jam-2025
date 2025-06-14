using UnityEngine;

public class MeleeEnemyApproachState : State
{
    private float weaponAttackTimer = 0f;
    private float weaponAttackTime;
    private MeleeEnemyStats stats;
    private MeleeEnemyStateMachine enemyStateMachine;

    public MeleeEnemyApproachState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as MeleeEnemyStateMachine;
    }

    public override void Enter()
    {
        stats = enemyStateMachine.GetStats();
        weaponAttackTime =
            stats.weaponAttackInterval
            + Random.Range(-stats.weaponAttackVariance, stats.weaponAttackVariance);
        weaponAttackTimer = 0f;

        stateMachine.smAnimator.SetBool("chasing", true);
    }

    public override void Exit()
    {
        stateMachine.smAnimator.SetBool("chasing", false);

        Vector2 attackDirection = (
            enemyStateMachine.GetPlayerTransform().position - stateMachine.transform.position
        ).normalized;

        enemyStateMachine.SetAttackDirection(attackDirection);
    }

    public override void Tick(float deltaTime)
    {
        // if (!stateMachine.canMove)
        // {
        //     return;
        // }

        // Vector3 directionToMove = (
        //     enemyStateMachine.GetPlayerTransform().position - stateMachine.transform.position
        // ).normalized;
        // directionToMove.z = 0f;

        // if (
        //     Vector2.Distance(
        //         enemyStateMachine.GetPlayerTransform().position,
        //         stateMachine.transform.position
        //     ) < stats.weaponAttackRange
        // )
        // {
        //     directionToMove = -(directionToMove * 0.75f);
        //     weaponAttackTimer += deltaTime;

        //     if (weaponAttackTimer > weaponAttackTime)
        //     {
        //         stateMachine.SwitchState(new MeleeEnemyAttackState(stateMachine));
        //         return;
        //     }
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
            directionToMove =
                -(
                    enemyStateMachine.GetPlayerTransform().position
                    - stateMachine.transform.position
                ).normalized * 0.75f;
            directionToMove.z = 0f;
        }
        else
        {
            weaponAttackTimer += deltaTime;

            if (weaponAttackTimer > weaponAttackTime)
            {
                stateMachine.SwitchState(new MeleeEnemyAttackState(stateMachine));
                return;
            }
        }

        stateMachine.transform.position += directionToMove * stats.movementSpeed * deltaTime;
    }
}
