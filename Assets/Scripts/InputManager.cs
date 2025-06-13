using UnityEngine;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    public static InputManager Instance { get; private set; }
    private Controls controls;

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogError("There's more than one InputManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();
    }

    public void OnNewaction(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
