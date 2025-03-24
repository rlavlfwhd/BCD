using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;
    private Color originalColor;
    public Color hoverColor = Color.yellow; // ������ ����

    void Start()
    {
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color; // ���� ���� ����
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���콺�� ��ư ���� ���� �� ������ ����
        buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // ���콺�� ��ư�� ���� �� ���� ���󺹱�
        buttonImage.color = originalColor;
    }
}
