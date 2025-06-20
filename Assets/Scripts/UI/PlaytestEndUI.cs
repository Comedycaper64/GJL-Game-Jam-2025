using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaytestEndUI : MonoBehaviour
{
    private int resultIndex = 0;

    [SerializeField]
    private Color positiveColour;

    [SerializeField]
    private Color middlingColour;

    [SerializeField]
    private Color negativeColour;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private TextMeshProUGUI playtimeText;

    [SerializeField]
    private TextMeshProUGUI attitudeText;

    [SerializeField]
    private TextMeshProUGUI feedbackText;

    [SerializeField]
    private TextMeshProUGUI changesText;

    [SerializeField]
    private DialogueUI dialogueUI;

    [SerializeField]
    private CanvasGroupFader endFader;

    [SerializeField]
    private CanvasGroupFader[] resultFaders;

    private void Start()
    {
        ConversationManager.OnPlaytestEnd += StartPlaytestEndUI;
    }

    private void OnDisable()
    {
        ConversationManager.OnPlaytestEnd -= StartPlaytestEndUI;
    }

    private void StartPlaytestEndUI()
    {
        endFader.ToggleFade(true);
        endFader.ToggleBlockRaycasts(true);
        EvaluatePlayerChoices();
    }

    private void EvaluatePlayerChoices()
    {
        dialogueUI.StartCallTimer(false);

        float timer = dialogueUI.GetCallTimer();

        TimeSpan time = TimeSpan.FromSeconds(timer);
        playtimeText.text = "Playtest Length ~ " + time.ToString(@"mm\:ss");

        EvaluateAttitude();
        EvaluateFeedback();
    }

    private void EvaluateAttitude()
    {
        int attitude = AttitudeManager.Instance.GetAttitude();
        if (attitude > 19)
        {
            attitudeText.text = "Very positive";
            attitudeText.color = positiveColour;
        }
        else if (attitude > 14)
        {
            attitudeText.text = "Mostly positive";
            attitudeText.color = positiveColour;
        }
        else if (attitude > 9)
        {
            attitudeText.text = "Mostly negative";
            attitudeText.color = negativeColour;
        }
        else
        {
            attitudeText.text = "Very negative";
            attitudeText.color = negativeColour;
        }
    }

    private void EvaluateFeedback()
    {
        FeedbackType feedbackType = FeedbackManager.Instance.GetHighestFeedbackType();

        switch (feedbackType)
        {
            case FeedbackType.praise:
                feedbackText.text = "Praising the game";
                feedbackText.color = positiveColour;
                break;
            case FeedbackType.constructive:
                feedbackText.text = "giving constructive feedback";
                feedbackText.color = middlingColour;
                break;
            case FeedbackType.critique:
                feedbackText.text = "critical of the game";
                feedbackText.color = negativeColour;
                break;
            case FeedbackType.silence:
                feedbackText.text = "silent";
                break;

            default:
                break;
        }

        int changesMade = FeedbackManager.Instance.GetChangesInfluenced();

        changesText.text = "Changes made: " + changesMade.ToString();
    }

    private IEnumerator DelayedNextButtonReenable()
    {
        yield return new WaitForSeconds(1f);

        nextButton.interactable = true;
    }

    private void LoadNextLevel()
    {
        //async load
    }

    public void NextSlide()
    {
        nextButton.interactable = false;

        resultFaders[resultIndex].ToggleFade(false);
        resultIndex++;
        resultFaders[resultIndex].ToggleFade(true);

        if (resultIndex >= (resultFaders.Length - 1))
        {
            LoadNextLevel();
        }
        else
        {
            StartCoroutine(DelayedNextButtonReenable());
        }
    }
}
