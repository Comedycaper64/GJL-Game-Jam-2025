using UnityEngine;

public class PlayerCursorPointer : MonoBehaviour
{
    [SerializeField]
    private float damp = 5f;
    private Vector3 mousePosition;
    private Vector3 mouseWorldPosition;

    private void Update()
    {
        RotateToMouse();
    }

    private void RotateToMouse()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Quaternion rotationAngle = Quaternion.LookRotation(
            mouseWorldPosition - transform.position,
            Vector3.forward
        );
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotationAngle,
            Time.deltaTime * damp
        );
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }

    public Vector3 GetCursorDirection()
    {
        return -transform.up;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = mouseWorldPosition;
        worldPosition.z = 0;
        return worldPosition;
    }
}
