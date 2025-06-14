using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DialogueUIEventArgs
{
    public DialogueUIEventArgs(string sentence, Action onTypingFinished)
    {
        this.sentence = sentence;
        this.onTypingFinished = onTypingFinished;
    }

    public string sentence;
    public Action onTypingFinished;
}

// public struct DialogueChoiceUIEventArgs
// {
//     public DialogueChoiceUIEventArgs(
//         DialogueChoiceSO dialogueChoice,
//         EventHandler<DialogueChoice> onDialogueChosen
//     )
//     {
//         this.dialogueChoice = dialogueChoice;
//         this.onDialogueChosen = onDialogueChosen;
//     }

//     public DialogueChoiceSO dialogueChoice;
//     public EventHandler<DialogueChoice> onDialogueChosen;
// }

public class DialogueManager : MonoBehaviour
{
    //private bool bLogActive = false;
    //private bool bIsSentenceTyping;
    //private bool bLoopToChoice;
    //private bool bAutoPlay = false;
    private float crossFadeTime = 0.1f;

    //private Coroutine autoPlayCoroutine;
    //private Coroutine animationCoroutine;
    //private ActorSO currentActor;
    //private DialogueChoiceSO currentChoice;
    //private int actorIndex;
    //private Animator[] actorAnimators;
    private string currentSentence;
    private AudioSource dialogueAudioSource;

    //private DialogueCameraDirector dialogueCameraDirector;
    //private ActorAnimatorMapper actorAnimatorMapper;
    private Queue<string> currentDialogue;

    //private Queue<AnimationClip> currentAnimations;
    //private Queue<float> currentAnimationCrossFadeTimes;
    //private Queue<float> currentAnimationTimes;
    //private Queue<CameraMode> currentCameraModes;
    private Queue<AudioClip> currentVoiceClips;

    //private bool disableCameraOnEnd;

    private Action onDialogueComplete;
    private Queue<Dialogue> dialogues;
    public static event Action OnFinishTypingDialogue;
    public static event EventHandler<bool> OnToggleDialogueUI;

    //public static event EventHandler<DialogueChoiceUIEventArgs> OnDisplayChoices;

    //public static event EventHandler<Sprite[]> OnChangeSprite;
    public static event EventHandler<DialogueUIEventArgs> OnDialogue;

    private void Awake()
    {
        dialogueAudioSource = GetComponent<AudioSource>();
    }

    private void OnDisable() { }

    public void PlayDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        this.onDialogueComplete = onDialogueComplete;
        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());
        //InputManager.Instance.OnShootAction += InputManager_OnShootAction;

        ToggleDialogueUI(true);
        TryPlayNextDialogue();
    }

    public void SkipCurrentDialogue()
    {
        DialogueSkipCleanup();

        EndDialogue(true);
    }

    // public void SkipCurrentChoice()
    // {
    //     DialogueSkipCleanup();
    //     ResetChoices();
    //     bLoopToChoice = false;

    //     EndDialogue(true);
    // }

    public void SkipDialogue(DialogueSO dialogueSO, Action onDialogueComplete)
    {
        DialogueSkipCleanup();

        dialogues = new Queue<Dialogue>(dialogueSO.GetDialogues());

        onDialogueComplete();
    }

    private void DialogueSkipCleanup()
    {
        dialogueAudioSource.Stop();
    }

    // public void DisplayChoices(DialogueChoiceSO dialogueChoiceSO, Action onDialogueComplete)
    // {
    //     this.onDialogueComplete = onDialogueComplete;
    //     currentChoice = dialogueChoiceSO;
    //     ToggleDialogueUI(true);
    //     DialogueChoiceUIEventArgs choiceUIEventArgs = new DialogueChoiceUIEventArgs(
    //         dialogueChoiceSO,
    //         PlayChoiceDialogue
    //     );
    //     OnDisplayChoices?.Invoke(this, choiceUIEventArgs);
    // }

    // private void PlayChoiceDialogue(object sender, DialogueChoice dialogueChoice)
    // {
    //     if (dialogueChoice.loopBackToChoice)
    //     {
    //         bLoopToChoice = true;
    //     }

    //     dialogues = new Queue<Dialogue>(
    //         currentChoice.GetDialogueAnswers()[dialogueChoice.correspondingDialogue].dialogueAnswers
    //     );
    //     InputManager.Instance.OnShootAction += InputManager_OnShootAction;

    //     TryPlayNextDialogue();
    // }

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

        bool playingVoiceClip = TryPlayVoiceClip();

        if (currentSentence == "")
        {
            ToggleDialogueUI(false);
        }
        else
        {
            ToggleDialogueUI(true);
            StartTypingSentence(playingVoiceClip);
        }
    }

    private bool TryPlayVoiceClip()
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

            // if (bAutoPlay)
            // {
            //     autoPlayCoroutine = StartCoroutine(
            //         DialogueAutoPlayTimer(
            //             voiceClip.length * (1 / currentActor.GetAudioMixerPitchMod())
            //         )
            //     );
            // }
            return true;
        }

        return false;
    }

    private IEnumerator DialogueAutoPlayTimer(float dialogueTime)
    {
        yield return new WaitForSeconds(dialogueTime);
        DisplayNextSentence();
    }

    private void StartTypingSentence(bool playingVoiceClip)
    {
        // bIsSentenceTyping = true;

        // bool playDialogueNoises = !playingVoiceClip;

        // OnDialogue?.Invoke(
        //     this,
        //     new DialogueUIEventArgs(
        //         currentActor,
        //         currentSentence,
        //         playDialogueNoises,
        //         FinishTypingSentence
        //     )
        // );
    }

    private IEnumerator AnimationPause(float pauseTime)
    {
        // InputManager.Instance.OnShootAction -= InputManager_OnShootAction;

        yield return new WaitForSeconds(pauseTime);

        // InputManager.Instance.OnShootAction += InputManager_OnShootAction;

        // animationCoroutine = null;

        // DisplayNextSentence();
    }

    private void FinishTypingSentence()
    {
        //bIsSentenceTyping = false;
    }

    private void ToggleDialogueUI(bool toggle)
    {
        OnToggleDialogueUI?.Invoke(this, toggle);
    }

    private void ResetChoices()
    {
        // DialogueChoiceUIEventArgs blankChoiceUIEventArgs = new DialogueChoiceUIEventArgs(
        //     null,
        //     null
        // );
        // OnDisplayChoices?.Invoke(this, blankChoiceUIEventArgs);
    }

    private void EndDialogue(bool skipping = false)
    {
        //InputManager.Instance.OnShootAction -= InputManager_OnShootAction;

        dialogueAudioSource.Stop();

        ToggleDialogueUI(false);

        ResetChoices();

        if (!skipping)
        {
            onDialogueComplete();
        }
        else
        {
            onDialogueComplete = null;
        }
    }

    private void InputManager_OnShootAction()
    {
        // if (bLogActive)
        // {
        //     return;
        // }

        // DisplayNextSentence();
    }
}
