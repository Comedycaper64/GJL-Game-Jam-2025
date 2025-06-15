using System;
using UnityEngine;

public class AreaEventTrigger : MonoBehaviour
{
    private bool eventTriggered = false;

    [SerializeField]
    private DialogueCluster eventDialogue;

    public static EventHandler<DialogueCluster> OnAreaEvent;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (eventTriggered)
        {
            return;
        }

        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && healthSystem.GetIsPlayer()
        )
        {
            OnAreaEvent?.Invoke(this, eventDialogue);
            eventTriggered = true;
        }
    }
}
