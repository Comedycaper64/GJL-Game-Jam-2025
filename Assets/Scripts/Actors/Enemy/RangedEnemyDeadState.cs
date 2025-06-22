public class RangedEnemyDeadState : State
{
    public RangedEnemyDeadState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.smAnimator.SetTrigger("death");
        stateMachine.ToggleInactive(true);
    }

    public override void Exit()
    {
        stateMachine.ToggleInactive(false);
    }

    public override void Tick(float deltaTime) { }
}
