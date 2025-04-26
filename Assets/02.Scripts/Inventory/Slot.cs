using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    [SerializeField] private Image icon;
    [SerializeField] private Image background;

    private static Color normalColor = Color.white;
    private static Color selectedColor = new Color(1f, 1f, 1f, 0.5f);

    public void SetItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.itemImage;
        icon.enabled = true;
        ResetHighlight();
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        ResetHighlight();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            bool isAlreadySelected = (Inventory.Instance.firstSelectedItem == item || Inventory.Instance.secondSelectedItem == item);

            Inventory.Instance.SelectItem(item);

            if (isAlreadySelected)
            {
                ClearAllHighlights();
            }
            else
            {
                HighlightSelectedSlot();
            }
        }
    }

    public void HighlightSelectedSlot()
    {
        Slot[] allSlots = FindObjectsOfType<Slot>();
        foreach (Slot slot in allSlots)
        {
            if (slot.item == null)
            {
                slot.background.color = normalColor;
                continue;
            }

            bool isSelected = slot.item == Inventory.Instance.firstSelectedItem || slot.item == Inventory.Instance.secondSelectedItem;
            slot.background.color = isSelected ? selectedColor : normalColor;
        }
    }

    public void ClearAllHighlights()
    {
        Slot[] allSlots = FindObjectsOfType<Slot>();
        foreach (Slot slot in allSlots)
        {
            slot.background.color = normalColor;
        }
    }

    private void ResetHighlight()
    {
        if (background != null)
            background.color = normalColor;
    }
}
