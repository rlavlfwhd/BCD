using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] Image image;
    private Item _item;
    private GameObject dragIcon;

    public Item item 
    { 
        get { return _item; }
        set {
            _item = value;
            if(_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
            }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null) return;

        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(transform.root, false);
        var img = dragIcon.AddComponent<Image>();
        img.sprite = item.itemImage;
        img.raycastTarget = false;
        img.SetNativeSize();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(dragIcon != null)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIcon != null)
            Destroy(dragIcon);

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            var dropTarget = result.gameObject.GetComponent<IDropTarget>();
            if (dropTarget != null)
            {
                Debug.Log(" UI ������� IDropTarget ã�� �� " + result.gameObject.name);
                dropTarget.OnItemDropped(item);
                return;
            }
        }

        Debug.Log(" UI ��� Ray�ε� DropTarget �� ã��");

        foreach (RaycastResult result in results)
        {
            // ���� ó��
            var dropTarget = result.gameObject.GetComponent<IDropTarget>();
            if (dropTarget != null)
            {
                Debug.Log("���� ������Ʈ�� ���");
                dropTarget.OnItemDropped(item);
                return;
            }

            // ���� ���� ����Ǿ����� Ȯ��
            var otherSlot = result.gameObject.GetComponent<Slot>();
            if (otherSlot != null && otherSlot != this && otherSlot.item != null)
            {
                Debug.Log($"�κ��丮 ���� �� ��� ����: {item.itemName} + {otherSlot.item.itemName}");
                Inventory.Instance.CombineItems(item, otherSlot.item);
                return;
            }
        }
    }


}
