using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableBook : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string bookColor; // ��: "Red", "Yellow", ...

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Transform originalParent;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root); // �巡�� �� �� ������
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.GetComponent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (transform.parent == transform.root)
            transform.SetParent(originalParent); // ��� �ȵ��� ��� ����ġ
    }
}
