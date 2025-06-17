using UnityEngine;

public class MeleeEnemyApproachState : State
{
    private float weaponAttackTimer = 0f;
    private float weaponAttackTime;
    private Rigidbody2D rb;
    private MeleeEnemyStats stats;
    private MeleeEnemyStateMachine enemyStateMachine;

    public MeleeEnemyApproachState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as MeleeEnemyStateMachine;
        rb = enemyStateMachine.GetRigidbody();
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
    }

    public override void Tick(float deltaTime)
    {
        float distanceToPlayer = Vector2.Distance(
            enemyStateMachine.GetPlayerTransform().position,
            stateMachine.transform.position
        );

        Vector2 playerDirection = (
            enemyStateMachine.GetPlayerTransform().position - stateMachine.transform.position
        ).normalized;

        enemyStateMachine.SetAttackDirection(playerDirection);

        Vector2 directionToMove = Vector2.zero;

        if (distanceToPlayer > stats.weaponAttackRange)
        {
            directionToMove = playerDirection;
        }
        else if (distanceToPlayer < stats.retreatRange)
        {
            weaponAttackTimer += deltaTime;
            directionToMove = -playerDirection * 0.75f;
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

        //stateMachine.transform.position += directionToMove * stats.movementSpeed * deltaTime;
        rb.MovePosition(rb.position + ((Vector2)directionToMove * stats.movementSpeed * deltaTime));
    }
}
