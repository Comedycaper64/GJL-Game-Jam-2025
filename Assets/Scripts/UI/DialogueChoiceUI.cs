using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoiceUI : MonoBehaviour
{
    private bool choiceActive = false;
    private float choiceTimer = 0f;
    private float choiceTime = 10f;

    [SerializeField]
    private CanvasGroupFader choiceButtonFader;

    [SerializeField]
    private Slider choiceTimeSlider;

    [SerializeField]
    private ChoiceButtonUI[] choiceButtons;

    public static EventHandler<int> OnDialogueChosen;

    private void Awake()
    {
        DialogueChoiceManager.OnDialogueChoice += DisplayDialogueChoices;
        ChoiceButtonUI.OnChoiceButton += DialogueChosen;
        InputManager.OnChooseDialogueAction += InputManager_OnChooseDialogue;

        choiceButtonFader.SetCanvasGroupAlpha(0f);
        choiceButtonFader.ToggleBlockRaycasts(false);
    }

    private void OnDisable()
    {
        DialogueChoiceManager.OnDialogueChoice -= DisplayDialogueChoices;
        ChoiceButtonUI.OnChoiceButton -= DialogueChosen;
        InputManager.OnChooseDialogueAction -= InputManager_OnChooseDialogue;
    }

    private void Update()
    {
        if (choiceActive)
        {
            choiceTimer += Time.deltaTime;

            float timeFractionValue = (choiceTime - choiceTimer) / choiceTime;
            choiceTimeSlider.value = timeFractionValue;

            float alphaFractionValue = Mathf.Clamp01(
                Mathf.InverseLerp(0f, 0.7f, timeFractionValue)
            );

            float alphaLerpValue = Mathf.Lerp(0.6f, 1f, alphaFractionValue);

            if (alphaLerpValue <= 1f)
            {
                choiceButtonFader.SetCanvasGroupAlpha(alphaLerpValue);
            }

            if (choiceTimer > choiceTime)
            {
                TimeUp();
            }
        }
    }

    private void ResetChoiceButtons()
    {
        foreach (ChoiceButtonUI button in choiceButtons)
        {
            button.ResetChoiceButton();
        }
    }

    private void TimeUp()
    {
        DialogueChosen(this, -1);
    }

    private void DialogueChosen(object sender, int index)
    {
        choiceActive = false;
        ResetChoiceButtons();
        choiceButtonFader.ToggleFade(false);
        choiceButtonFader.ToggleBlockRaycasts(false);

        OnDialogueChosen?.Invoke(this, index);
    }

    private void DisplayDialogueChoices(object sender, DialogueChoiceSO choiceSO)
    {
        choiceActive = true;
        choiceTimer = 0f;
        ResetChoiceButtons();

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < choiceSO.GetDialoguesChoices().Length; i++)
        {
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].SetupChoiceButton(choiceSO.GetDialoguesChoices()[i].dialogueChoice, i);
        }

        choiceButtonFader.ToggleFade(true);
        choiceButtonFader.ToggleBlockRaycasts(true);
    }

    private void InputManager_OnChooseDialogue(object sender, int choiceIndex)
    {
        if (!choiceActive)
        {
            return;
        }

        DialogueChosen(this, choiceIndex - 1);
    }
}
