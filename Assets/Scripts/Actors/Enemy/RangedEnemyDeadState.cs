using UnityEngine;

public class RangedEnemyDeadState : State
{
    private RangedEnemyStateMachine enemyStateMachine;

    public RangedEnemyDeadState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as RangedEnemyStateMachine;
    }

    public override void Enter()
    {
        // stateMachine.isDead = true;
        // stateMachine.bodyCollider.enabled = false;
        // stateMachine.animator.SetTrigger("die");
        stateMachine.ToggleInactive(true);
    }

    public override void Exit()
    {
        //stateMachine.isDead = false;
        stateMachine.ToggleInactive(false);
    }

    public override void Tick(float deltaTime) { }
}
