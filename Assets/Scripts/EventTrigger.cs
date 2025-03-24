using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Color originalColor;
    public Color hoverColor = Color.yellow; // 빛나는 색상

    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color; // 원래 색상 저장
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스가 버튼 위에 있을 때 색상을 변경
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 버튼을 떠날 때 색상 원상복귀
        buttonImage.color = originalColor;
    }
}
