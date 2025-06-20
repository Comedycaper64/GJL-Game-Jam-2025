using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    private bool timerStarted = false;
    private float timer = 0f;
    private float dialogueFadeTime = 0.25f;

    private Coroutine dialogueDisplayCoroutine;

    [SerializeField]
    private GameObject speakerIndicator;

    [SerializeField]
    private CanvasGroupFader dialogueFader;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private TextMeshProUGUI timerText;

    private void Awake()
    {
        DialogueManager.OnDialogue += DialogueManager_OnDialogue;

        ClearDialogueText();
        dialogueFader.SetCanvasGroupAlpha(0f);
        dialogueFader.ToggleBlockRaycasts(false);
        speakerIndicator.SetActive(false);

        //Temp
        //timerStarted = true;
    }

    private void OnDisable()
    {
        DialogueManager.OnDialogue -= DialogueManager_OnDialogue;
    }

    private void Update()
    {
        if (timerStarted)
        {
            timer += Time.deltaTime;

            TimeSpan time = TimeSpan.FromSeconds(timer);
            timerText.text = time.ToString(@"mm\:ss");
        }
    }

    private void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    private IEnumerator DisplaySentence(DialogueUIEventArgs dialogueUIEventArgs)
    {
        dialogueText.text = dialogueUIEventArgs.sentence;
        speakerIndicator.SetActive(true);

        dialogueFader.ToggleFade(true);

        //displayingDialogue = true;

        yield return new WaitForSeconds(dialogueUIEventArgs.displayDuration - dialogueFadeTime);

        dialogueFader.ToggleFade(false);
        speakerIndicator.SetActive(false);
        yield return new WaitForSeconds(dialogueFadeTime);

        //displayingDialogue = false;
    }

    public void StartCallTimer(bool toggle = true)
    {
        timerStarted = toggle;
    }

    public float GetCallTimer()
    {
        return timer;
    }

    private void DialogueManager_OnDialogue(object sender, DialogueUIEventArgs dialogueArgs)
    {
        if (dialogueDisplayCoroutine != null)
        {
            StopCoroutine(dialogueDisplayCoroutine);
        }

        dialogueDisplayCoroutine = StartCoroutine(DisplaySentence(dialogueArgs));
    }
}
