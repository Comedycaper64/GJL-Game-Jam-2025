using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] localTexts;
    private static TextMeshProUGUI[] texts;

    private void Awake()
    {
        texts = localTexts;
    }

    public static void SetCursorText(Vector2 screenPosition)
    {
        texts[0].text = "Cursor Screen Position: " + screenPosition;
    }

    public static void SetPauseInvoked()
    {
        texts[1].text = "Paused Invoked: " + Time.time;
    }

    public static void SetPauseInput()
    {
        texts[2].text = "Paused Input: " + Time.time;
    }
}
