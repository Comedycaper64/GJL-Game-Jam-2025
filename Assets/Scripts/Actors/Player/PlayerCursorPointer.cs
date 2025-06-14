using UnityEngine;

public class PlayerCursorPointer : MonoBehaviour
{
    private Vector3 mousePosition;
    private Vector3 mouseWorldPosition;

    private void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 0;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.LookAt(mouseWorldPosition, Vector3.forward);
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
    }

    public Vector3 GetCursorDirection()
    {
        return transform.up;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = mouseWorldPosition;
        worldPosition.z = 0;
        return worldPosition;
    }
}
