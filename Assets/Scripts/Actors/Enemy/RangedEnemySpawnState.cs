public class RangedEnemySpawnState : State
{
    float stateTime = 2f;
    float stateTimer;
    private RangedEnemyStateMachine enemyStateMachine;

    public RangedEnemySpawnState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as RangedEnemyStateMachine;
    }

    public override void Enter()
    {
        stateTimer = stateTime;
        enemyStateMachine.ToggleVisual(true);
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
            stateMachine.SwitchState(new RangedEnemyAimState(stateMachine));
            return;
        }
    }
}
