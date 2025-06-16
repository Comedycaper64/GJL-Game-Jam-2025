using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    [SerializeField]
    private Transform checkPointLocation;

    public Transform GetCheckPoint()
    {
        return checkPointLocation;
    }
}
