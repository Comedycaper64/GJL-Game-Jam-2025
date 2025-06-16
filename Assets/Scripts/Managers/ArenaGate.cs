using UnityEngine;

public class ArenaGate : MonoBehaviour
{
    private bool spriteActive = false;
    private bool fade = false;
    private float newAlpha;

    [SerializeField]
    private Collider2D gateCollider;

    [SerializeField]
    private SpriteRenderer gateSprite;

    private void Start()
    {
        gateCollider.enabled = false;
        gateSprite.color = new Color(1f, 1f, 1f, 0f);
    }

    private void Update()
    {
        if (fade)
        {
            if (spriteActive)
            {
                newAlpha = gateSprite.color.a + Time.deltaTime;
                gateSprite.color = new Color(1f, 1f, 1f, newAlpha);

                if (newAlpha >= 1f)
                {
                    fade = false;
                }
            }
            else
            {
                newAlpha = gateSprite.color.a - Time.deltaTime;
                gateSprite.color = new Color(1f, 1f, 1f, newAlpha);

                if (newAlpha <= 0f)
                {
                    fade = false;
                }
            }
        }
    }

    public void ToggleGate(bool toggle)
    {
        gateCollider.enabled = toggle;
        spriteActive = toggle;
        fade = true;
    }
}
