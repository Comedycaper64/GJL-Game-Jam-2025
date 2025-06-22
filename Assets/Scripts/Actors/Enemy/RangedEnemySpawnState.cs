public class RangedEnemySpawnState : State
{
    private float stateTime = 2f;
    private float stateTimer;
    private RangedEnemyStateMachine enemyStateMachine;

    public RangedEnemySpawnState(StateMachine stateMachine)
        : base(stateMachine)
    {
        enemyStateMachine = stateMachine as RangedEnemyStateMachine;
    }

    public override void Enter()
    {
        stateTimer = stateTime;
        stateMachine.smAnimator.SetTrigger("spawn");
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
