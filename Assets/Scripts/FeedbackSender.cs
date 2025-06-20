using System;
using UnityEngine;

public class FeedbackSender : MonoBehaviour
{
    [SerializeField]
    private DialogueCluster dialogueCluster;

    public static EventHandler<DialogueCluster> OnSendFeedback;

    public void SendFeedback(DialogueCluster customCluster = null)
    {
        if (customCluster != null)
        {
            OnSendFeedback.Invoke(this, customCluster);
        }
        else
        {
            OnSendFeedback.Invoke(this, dialogueCluster);
        }
    }
}
