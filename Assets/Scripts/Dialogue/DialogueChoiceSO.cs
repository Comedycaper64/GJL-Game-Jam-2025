using System;
using UnityEngine;

[Serializable]
public struct DialogueChoice
{
    [TextArea]
    public string dialogueChoice;
    public string feedbackKey;
    public int feedbackValue;
    public int attitudeModifier;
}

[Serializable]
[CreateAssetMenu(fileName = "DialogueChoice", menuName = "Dialogue/DialogueChoiceSO", order = 0)]
public class DialogueChoiceSO : ConversationNode
{
    [SerializeField]
    private DialogueChoice[] dialogueChoices;

    public DialogueChoice[] GetDialoguesChoices()
    {
        return dialogueChoices;
    }
}
