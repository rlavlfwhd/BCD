using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonTextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text targetText;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    private bool isHovering = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        if (targetText != null)
            targetText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        if (targetText != null)
            targetText.color = normalColor;
    }
}
