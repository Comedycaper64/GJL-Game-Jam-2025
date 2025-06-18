using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(
    fileName = "DialogueFeedback",
    menuName = "Dialogue/DialogueFeedbackSO",
    order = 1
)]
public class DialogueFeedbackSO : ConversationNode
{
    [SerializeField]
    private string feedbackKey;

    [SerializeField]
    private int feedbackValue;

    public string GetKey()
    {
        return feedbackKey;
    }

    public int GetValue()
    {
        return feedbackValue;
    }
}
