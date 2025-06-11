using UnityEngine;

public class GemSlotSimple : MonoBehaviour, IClickablePuzzle
{
    public string correctGemName;
    [HideInInspector] public string currentGemName = "";

    public SpriteRenderer slotRenderer;
    public GemBoxPuzzleManager puzzleManager;

    public void OnClickPuzzle()
    {
        puzzleManager.TryPlaceGemToSlot(this);
    }

    public void UpdateSlotUI(Sprite gemSprite)
    {
        if (slotRenderer != null)
        {
            slotRenderer.sprite = gemSprite;
            slotRenderer.color = (gemSprite != null) ? Color.white : new Color(1, 1, 1, 0);
        }
    }
}
