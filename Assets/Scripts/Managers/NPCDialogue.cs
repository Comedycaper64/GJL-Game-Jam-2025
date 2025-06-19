using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    private bool dialogueStarted = false;
    private float timeBetweenLetterTyping = 0.04f;
    private float finishTypingWaitTime = 3f;
    private Coroutine dialogueCoroutine;

    private Queue<string> spokenDialogue;

    [SerializeField]
    private DialogueSO dialogueSO;

    [SerializeField]
    private GameObject npcLeaveAreaCluster;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    [SerializeField]
    private CanvasGroupFader textBoxFader;

    private void Awake()
    {
        spokenDialogue = new Queue<string>(dialogueSO.GetDialogue().dialogue);
        dialogueStarted = false;
        textBoxFader.SetCanvasGroupAlpha(0f);
    }

    private void OnDisable()
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dialogueStarted)
        {
            return;
        }

        if (
            other.TryGetComponent<HealthSystem>(out HealthSystem healthSystem)
            && healthSystem.GetIsPlayer()
        )
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        dialogueStarted = true;
        textBoxFader.ToggleFade(true);
        dialogueCoroutine = StartCoroutine(PlayNextDialogue(spokenDialogue.Dequeue()));
    }

    public void StopDialogue()
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        npcLeaveAreaCluster.SetActive(false);
    }

    private IEnumerator PlayNextDialogue(string dialogue)
    {
        dialogueText.text = dialogue;

        dialogueText.maxVisibleCharacters = 0;

        yield return null;

        string parsedText = dialogueText.GetParsedText();

        for (int i = 0; i < parsedText.Length; i++)
        {
            dialogueText.maxVisibleCharacters++;

            yield return new WaitForSeconds(timeBetweenLetterTyping);
        }

        yield return new WaitForSeconds(finishTypingWaitTime);

        if (spokenDialogue.TryDequeue(out string nextDialogue))
        {
            dialogueCoroutine = StartCoroutine(PlayNextDialogue(nextDialogue));
        }
        else
        {
            textBoxFader.ToggleFade(false);
        }
    }
}
