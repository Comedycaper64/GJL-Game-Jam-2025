public class EnemyInactiveState : State
{
    public EnemyInactiveState(StateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.ToggleInactive(true);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
