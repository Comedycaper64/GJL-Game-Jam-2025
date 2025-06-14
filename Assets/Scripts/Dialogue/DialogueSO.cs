using System;
using UnityEngine;

[Serializable]
public struct Dialogue
{
    [TextArea]
    public string[] dialogue;

    public AudioClip[] voiceClip;
}

[Serializable]
[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueSO", order = 0)]
public class DialogueSO : CinematicNode
{
    [SerializeField]
    private Dialogue[] dialogues;

    public Dialogue[] GetDialogues()
    {
        return dialogues;
    }
}
