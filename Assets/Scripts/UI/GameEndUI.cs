using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameEndUI : PlaytestEndUI
{
    private int constructiveThreshold = 10;
    private int driveThreshold = 15;
    private bool constructivePlayer = false;
    private bool drivenDeveloper = false;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private TextMeshProUGUI constructiveText;

    [SerializeField]
    private TextMeshProUGUI developerAttitudeText;

    [SerializeField]
    private TextMeshProUGUI gameFateText;

    protected override void EvaluatePlayerChoices()
    {
        base.EvaluatePlayerChoices();

        EvaluateConstructiveChoices();
        EvaluateDeveloperDrive();
        EvaluateGameFate();
    }

    private void EvaluateConstructiveChoices()
    {
        int constructiveChoices = FeedbackManager.Instance.GetChangesInfluenced();

        if (constructiveChoices >= constructiveThreshold)
        {
            constructivePlayer = true;
        }

        if (constructivePlayer)
        {
            constructiveText.text =
                "Across the playtests you gave enough constructive criticism to significantly improve the game...";
        }
        else
        {
            constructiveText.text =
                "Across the playtests you did not give enough constructive criticism to significantly improve the game...";
        }
    }

    private void EvaluateDeveloperDrive()
    {
        int developerDrive = AttitudeManager.Instance.GetAttitude();

        if (developerDrive >= driveThreshold)
        {
            drivenDeveloper = true;
        }

        if (drivenDeveloper)
        {
            developerAttitudeText.text =
                "Because of your comments, the developer felt confident enough to finish the game...";
        }
        else
        {
            developerAttitudeText.text =
                "Because of your comments, the developer did not feel confident enough to finish the game...";
        }
    }

    private void EvaluateGameFate()
    {
        if (drivenDeveloper && constructivePlayer)
        {
            gameFateText.text =
                "And so the game came out... \nIn a good state\n\nAnd yet, it was not interesting or good enough to draw any attention";
        }
        else if (drivenDeveloper)
        {
            gameFateText.text =
                "And so the game came out... \nIn a poor state\n\nIt was not interesting or good enough to draw any attention";
        }
        else
        {
            gameFateText.text = "And so the game... \nDid not come out";
        }
    }

    public void RestartGame()
    {
        Destroy(FeedbackManager.Instance);
        Destroy(AttitudeManager.Instance);

        SceneManager.LoadScene(0);
    }

    private IEnumerator MusicChange()
    {
        audioManager.StopMusic();
        yield return new WaitForSeconds(2f);
        audioManager.SetEndingTrack();
        audioManager.StartMusic();
    }

    public override void NextSlide()
    {
        if (resultIndex == 4)
        {
            StartCoroutine(MusicChange());
        }

        nextButton.interactable = false;

        resultFaders[resultIndex].ToggleFade(false);
        resultFaders[resultIndex].ToggleBlockRaycasts(false);
        resultIndex++;
        resultFaders[resultIndex].ToggleFade(true);
        resultFaders[resultIndex].ToggleBlockRaycasts(true);

        if (resultIndex >= (resultFaders.Length - 1))
        {
            nextButton.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(DelayedNextButtonReenable());
        }
    }
}
