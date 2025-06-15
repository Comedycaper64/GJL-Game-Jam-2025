using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    public static InputManager Instance { get; private set; }
    public static bool disableInputs = false;

    public Vector2 MovementValue { get; private set; }
    public static Action OnAttackEvent;
    public static Action OnFeedbackEvent;
    public static Action OnAbsorbEvent;
    public static Action OnAbsorbReleaseEvent;
    public static Action OnDashEvent;
    public static Action OnMenuEvent;
    public static Action OnSkipEvent;

    private Controls controls;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one InputManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            MovementValue = Vector2.zero;
            return;
        }

        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
    }

    public void OnFeedback(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }
        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
    }

    public void OnAbsorb(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnAbsorbEvent?.Invoke();
        }
        else if (context.canceled)
        {
            OnAbsorbReleaseEvent?.Invoke();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }
        if (context.performed)
        {
            OnDashEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMenuEvent?.Invoke();
        }
    }

    public void OnSkip(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSkipEvent?.Invoke();
        }
    }
}
