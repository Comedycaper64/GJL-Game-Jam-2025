using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(
    fileName = "DialogueFeedbackUnlock",
    menuName = "Dialogue/DialogueFeedbackUnlockSO",
    order = 1
)]
public class DialogueFeedbackUnlockSO : ConversationNode
{
    public int feedbackUnlockIndex = 0;
}
