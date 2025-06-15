using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DialogueUIEventArgs
{
    public DialogueUIEventArgs(string sentence, float displayDuration)
    {
        this.sentence = sentence;
        this.displayDuration = displayDuration;
    }

    public string sentence;
    public float displayDuration;
}

public class DialogueManager : MonoBehaviour
{
    private string currentSentence;
    private AudioSource dialogueAudioSource;

    private Queue<string> currentDialogue;
    private Queue<AudioClip> currentVoiceClips;

    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;
    private Coroutine autoPlayCoroutine;

    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        dialogueAudioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        InputManager.OnSkipEvent -= SkipCurrentDialogue;
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());

        InputManager.OnSkipEvent += SkipCurrentDialogue;

        //ToggleDialogueUI(true);
        TryPlayNextDialogue();
    }

    public void SkipCurrentDialogue()
    {
        DialogueSkipCleanup();

        DisplayNextSentence();
    }

    private void DialogueSkipCleanup()
    {
        dialogueAudioSource.Stop();

        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
        }
    }

    private void TryPlayNextDialogue()
    {
        if (!dialogues.TryDequeue(out Dialogue dialogueNode))
        {
            EndDialogue();
            return;
        }

        currentDialogue = new Queue<string>(dialogueNode.dialogue);

        if (dialogueNode.voiceClip != null)
        {
            currentVoiceClips = new Queue<AudioClip>(dialogueNode.voiceClip);
        }
        else
        {
            currentVoiceClips = new Queue<AudioClip>();
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        // if (bIsSentenceTyping)
        // {
        //     //Debug.Log("We're typin here!");
        //     OnFinishTypingDialogue?.Invoke();
        //     return;
        // }

        if (!currentDialogue.TryDequeue(out currentSentence))
        {
            TryPlayNextDialogue();
            return;
        }

        float voiceClipLength = TryPlayVoiceClip();

        if (currentSentence == "")
        {
            //ToggleDialogueUI(false);
        }
        else
        {
            //ToggleDialogueUI(true);

            OnDialogue?.Invoke(this, new DialogueUIEventArgs(currentSentence, voiceClipLength));
        }
    }

    private float TryPlayVoiceClip()
    {
        if (dialogueAudioSource.isPlaying)
        {
            dialogueAudioSource.Stop();
        }

        if (currentVoiceClips.TryDequeue(out AudioClip voiceClip) && (voiceClip != null))
        {
            dialogueAudioSource.clip = voiceClip;

            // dialogueAudioSource.volume =
            //     PlayerOptions.GetMasterVolume() * PlayerOptions.GetVoiceVolume();

            dialogueAudioSource.Play();

            autoPlayCoroutine = StartCoroutine(DialogueAutoPlayTimer(voiceClip.length));

            return voiceClip.length;
        }

        return -1f;
    }

    private IEnumerator DialogueAutoPlayTimer(float dialogueTime)
    {
        yield return new WaitForSeconds(dialogueTime);
        DisplayNextSentence();
    }

    // private void ToggleDialogueUI(bool toggle)
    // {
    //     OnToggleDialogueUI?.Invoke(this, toggle);
    // }

    private void EndDialogue(bool skipping = false)
    {
        InputManager.OnSkipEvent -= SkipCurrentDialogue;

        dialogueAudioSource.Stop();

        //ToggleDialogueUI(false);

        if (!skipping)
        {
            onDialogueComplete();
        }
        else
        {
            onDialogueComplete = null;
        }
    }
}
