using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaytestEndUI : MonoBehaviour
{
    protected int resultIndex = 0;

    [SerializeField]
    protected Color positiveColour;

    [SerializeField]
    protected Color middlingColour;

    [SerializeField]
    protected Color negativeColour;

    [SerializeField]
    protected Button nextButton;

    [SerializeField]
    protected TextMeshProUGUI playtimeText;

    [SerializeField]
    protected TextMeshProUGUI attitudeText;

    [SerializeField]
    protected TextMeshProUGUI feedbackText;

    [SerializeField]
    protected TextMeshProUGUI changesText;

    [SerializeField]
    protected DialogueUI dialogueUI;

    [SerializeField]
    protected CanvasGroupFader endFader;

    [SerializeField]
    protected CanvasGroupFader[] resultFaders;

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

    protected virtual void EvaluatePlayerChoices()
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
        if (attitude > 18)
        {
            attitudeText.text = "Very positive";
            attitudeText.color = positiveColour;
        }
        else if (attitude > 13)
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

        changesText.text = changesMade.ToString() + " changes";
    }

    protected IEnumerator DelayedNextButtonReenable()
    {
        yield return new WaitForSeconds(1f);

        nextButton.interactable = true;
    }

    private IEnumerator LoadNextLevel()
    {
        int nextSceneBuild = 1;

        if (FeedbackManager.Instance.TryGetDictionaryValue("Style", out int val))
        {
            if (val == 1)
            {
                nextSceneBuild = 2;
            }
        }
        //evaluate next level
        //async load

        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync(nextSceneBuild);
    }

    public virtual void NextSlide()
    {
        nextButton.interactable = false;

        resultFaders[resultIndex].ToggleFade(false);

        resultIndex++;
        resultFaders[resultIndex].ToggleFade(true);

        if (resultIndex >= (resultFaders.Length - 1))
        {
            StartCoroutine(LoadNextLevel());
        }
        else
        {
            StartCoroutine(DelayedNextButtonReenable());
        }
    }
}
