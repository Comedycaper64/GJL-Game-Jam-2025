using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    public Animator smAnimator;

    void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }

    public virtual void SwitchState(State newState)
    {
        if (currentState?.GetType() == newState.GetType())
        {
            return;
        }

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public abstract void ToggleInactive(bool toggle);
}
