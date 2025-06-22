using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    private bool gameStarted = false;
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
    public static Action OnRespawnEvent;
    public static EventHandler<int> OnChooseDialogueAction;

    private Controls controls;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        disableInputs = true;
    }

    private void OnEnable()
    {
        PauseManager.OnPauseGame += ToggleDisableInputs;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseGame -= ToggleDisableInputs;
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
            OnFeedbackEvent?.Invoke();
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

    public void OnSelectDialogue(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }
        if (!context.performed)
        {
            return;
        }
        else
        {
            Vector2 choice = context.ReadValue<Vector2>();

            int dialogueChoice = 1;

            if (choice.y < 0f)
            {
                dialogueChoice = 2;
            }
            else if (choice.x < 0f)
            {
                dialogueChoice = 3;
            }
            else if (choice.x > 0f)
            {
                return;
            }

            OnChooseDialogueAction?.Invoke(this, dialogueChoice);
        }
    }

    public void OnRespawn(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }
        if (!context.performed)
        {
            return;
        }
        else
        {
            OnRespawnEvent?.Invoke();
        }
    }

    private void ToggleDisableInputs(object sender, bool toggle)
    {
        if (!gameStarted)
        {
            return;
        }

        disableInputs = toggle;
    }

    public void GameStart()
    {
        gameStarted = true;
        disableInputs = false;
    }

    public void GameEnd()
    {
        gameStarted = false;
        disableInputs = true;
    }
}
