using System;
using UnityEngine;

[Serializable]
public struct DialogueChoice
{
    public DialogueChoice(
        string dialogueChoice,
        string feedbackKey,
        int feedbackValue,
        int attitudeModifier,
        FeedbackType feedbackType = FeedbackType.na
    )
    {
        this.dialogueChoice = dialogueChoice;
        this.feedbackKey = feedbackKey;
        this.feedbackValue = feedbackValue;
        this.attitudeModifier = attitudeModifier;
        this.feedbackType = feedbackType;
    }

    [TextArea]
    public string dialogueChoice;
    public string feedbackKey;
    public int feedbackValue;
    public int attitudeModifier;
    public FeedbackType feedbackType;
}

[Serializable]
[CreateAssetMenu(fileName = "DialogueChoice", menuName = "Dialogue/DialogueChoiceSO", order = 1)]
public class DialogueChoiceSO : ConversationNode
{
    private int defaultAttitudeModifier = -1;

    [SerializeField]
    private DialogueChoice[] dialogueChoices;

    public DialogueChoice[] GetDialoguesChoices()
    {
        return dialogueChoices;
    }

    public int GetDefaultAttitudeModifier()
    {
        return defaultAttitudeModifier;
    }
}
