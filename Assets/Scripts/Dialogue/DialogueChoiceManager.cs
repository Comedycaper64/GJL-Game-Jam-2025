using System;
using UnityEngine;

public class DialogueChoiceManager : MonoBehaviour
{
    private Action onChoiceComplete;
    private DialogueChoice[] currentChoices;

    public static EventHandler<DialogueChoiceSO> OnDialogueChoice;

    public static EventHandler<FeedbackType> OnFeedbackType;

    private void Awake()
    {
        DialogueChoiceUI.OnDialogueChosen += HandleDialogueChoice;
    }

    private void OnDisable()
    {
        DialogueChoiceUI.OnDialogueChosen -= HandleDialogueChoice;
    }

    public void DisplayChoices(DialogueChoiceSO choiceSO, Action onChoiceComplete)
    {
        this.onChoiceComplete = onChoiceComplete;
        currentChoices = choiceSO.GetDialoguesChoices();

        OnDialogueChoice?.Invoke(this, choiceSO);
    }

    private void HandleDialogueChoice(object sender, int choiceIndex)
    {
        DialogueChoice chosenDialogue;
        if (choiceIndex < 0)
        {
            chosenDialogue = new DialogueChoice("", currentChoices[0].feedbackKey, -1, -1);
            OnFeedbackType?.Invoke(this, FeedbackType.silence);
        }
        else
        {
            chosenDialogue = currentChoices[choiceIndex];
        }

        AttitudeManager.Instance.AlterAttitude(chosenDialogue.attitudeModifier);

        FeedbackManager.Instance.SetDictionaryValue(
            chosenDialogue.feedbackKey,
            chosenDialogue.feedbackValue
        );

        if (chosenDialogue.feedbackType != FeedbackType.na)
        {
            OnFeedbackType?.Invoke(this, chosenDialogue.feedbackType);
        }

        onChoiceComplete();
        currentChoices = null;
        onChoiceComplete = null;
    }
}
