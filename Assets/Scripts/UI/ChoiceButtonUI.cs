using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButtonUI : MonoBehaviour
{
    private int choiceIndex = -1;

    [SerializeField]
    private TextMeshProUGUI buttonText;

    [SerializeField]
    private Button button;

    public static EventHandler<int> OnChoiceButton;

    public void SetupChoiceButton(string choiceDialogue, int choiceIndex)
    {
        buttonText.text = choiceDialogue;
        this.choiceIndex = choiceIndex;

        button.interactable = true;
    }

    public void ResetChoiceButton()
    {
        choiceIndex = -1;

        button.interactable = false;
    }

    public void ButtonChosen()
    {
        OnChoiceButton?.Invoke(this, choiceIndex);
    }
}
