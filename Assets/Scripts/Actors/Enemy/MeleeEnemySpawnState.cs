using UnityEngine;

public class MeleeEnemySpawnState : State
{
    float stateTime = 2f;
    float stateTimer;
    private MeleeEnemyStateMachine enemyStateMachine;

    public MeleeEnemySpawnState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as MeleeEnemyStateMachine;
    }

    public override void Enter()
    {
        stateTimer = stateTime;

        //stateMachine.animator.SetTrigger("spawn");
    }

    public override void Exit()
    {
        enemyStateMachine.ToggleCollider(true);
    }

    public override void Tick(float deltaTime)
    {
        stateTimer -= deltaTime;
        if (stateTimer <= 0f)
        {
            stateMachine.SwitchState(new MeleeEnemyApproachState(stateMachine));
            return;
        }
    }
}
