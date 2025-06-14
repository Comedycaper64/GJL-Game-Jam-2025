using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Animator bodyAnimator;

    [SerializeField]
    private Transform visualTransform;
    private InputManager inputManager;
    private bool canMove;
    private float movementSpeed;

    private void Awake()
    {
        canMove = true;
        movementSpeed = GetComponent<PlayerStats>().movementSpeed;
    }

    private void Start()
    {
        inputManager = InputManager.Instance;
    }

    private void Update()
    {
        Vector2 movementValue = Vector2.zero;
        if (canMove)
        {
            movementValue = Move();
        }
        //bodyAnimator.SetFloat("Speed", Mathf.Abs(movementValue.x) + Mathf.Abs(movementValue.y));
    }

    private Vector2 Move()
    {
        Vector2 movementValue = inputManager.MovementValue;
        transform.position +=
            new Vector3(inputManager.MovementValue.x, inputManager.MovementValue.y)
            * movementSpeed
            * Time.deltaTime;

        if (inputManager.MovementValue.x < 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (inputManager.MovementValue.x > 0)
        {
            visualTransform.eulerAngles = new Vector3(0, 0, 0);
        }

        return movementValue;
    }

    public void ToggleCanMove(bool enable)
    {
        canMove = enable;
    }
}
