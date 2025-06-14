using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    [SerializeField]
    private Transform localPlayerTransform;
    public static Transform PlayerTransform;

    private void Awake()
    {
        PlayerTransform = localPlayerTransform;
    }
}
