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
    private bool voicePaused = false;
    private string currentSentence;
    private AudioSource dialogueAudioSource;

    private Queue<string> currentDialogue;
    private Queue<AudioClip> currentVoiceClips;

    private Action onDialogueComplete;
    private Dialogue dialogues;
    private Coroutine autoPlayCoroutine;

    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        dialogueAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PauseManager.OnPauseGame += PauseVA;
    }

    private void OnDisable()
    {
        InputManager.OnSkipEvent -= SkipCurrentDialogue;
        PauseManager.OnPauseGame -= PauseVA;
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
        currentDialogue = new Queue<string>(dialogues.dialogue);

        if (dialogues.voiceClip != null)
        {
            currentVoiceClips = new Queue<AudioClip>(dialogues.voiceClip);
        }
        else
        {
            currentVoiceClips = new Queue<AudioClip>();
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (!currentDialogue.TryDequeue(out currentSentence))
        {
            EndDialogue();
            return;
        }

        float voiceClipLength = TryPlayVoiceClip();

        if (currentSentence != "")
        {
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

            dialogueAudioSource.volume = PlayerOptions.GetVoiceVolume();

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

    private void EndDialogue(bool skipping = false)
    {
        InputManager.OnSkipEvent -= SkipCurrentDialogue;

        dialogueAudioSource.Stop();

        if (!skipping)
        {
            onDialogueComplete();
        }
        else
        {
            onDialogueComplete = null;
        }
    }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = dialogueSO.GetDialogue();

        InputManager.OnSkipEvent += SkipCurrentDialogue;

        TryPlayNextDialogue();
    }

    public void SkipCurrentDialogue()
    {
        DialogueSkipCleanup();

        DisplayNextSentence();
    }

    private void PauseVA(object sender, bool pause)
    {
        if (!pause && voicePaused)
        {
            dialogueAudioSource.Play();
            voicePaused = false;
        }

        if (dialogueAudioSource.isPlaying)
        {
            if (pause)
            {
                dialogueAudioSource.Pause();
                voicePaused = true;
            }
        }
    }
}
