using System;
using UnityEngine;

public class FeedbackSender : MonoBehaviour
{
    [SerializeField]
    private DialogueCluster dialogueCluster;

    public static EventHandler<DialogueCluster> OnSendFeedback;

    public void SendFeedback()
    {
        OnSendFeedback.Invoke(this, dialogueCluster);
    }
}
