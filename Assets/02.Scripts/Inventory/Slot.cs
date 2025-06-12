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

        var existing = GetComponent<LetterItemBehavior>();
        if (existing != null)
            Destroy(existing);

        if (item != null && item.itemName == "Letter")
        {
            var letter = gameObject.AddComponent<LetterItemBehavior>();
            letter.letterPaperRoot = GameObject.Find("LetterContents");
            GameObject[] objs = new GameObject[4];
            for (int i = 0; i < 4; i++)
                objs[i] = letter.letterPaperRoot.transform.GetChild(i + 1).gameObject;
            letter.letterPaperObjects = objs;
            letter.darkFilterImage = letter.letterPaperRoot.transform.GetChild(0).gameObject;
            Item[] jewels = new Item[4];
            jewels[0] = Resources.Load<Item>("Items/Gem_Pink");
            jewels[1] = Resources.Load<Item>("Items/Gem_Red");
            jewels[2] = Resources.Load<Item>("Items/Gem_Brown");
            jewels[3] = Resources.Load<Item>("Items/Gem_White");
            letter.jewelItems = jewels;

            var btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
        }
        else if (item != null && item.itemName == "Note")
        {
            var note = gameObject.AddComponent<NoteItemBehavior>();
            note.noteViewRoot = GameObject.Find("NoteView");
            note.darkFilterImage = note.noteViewRoot.transform.GetChild(0).gameObject;
            note.noteObject = note.noteViewRoot.transform.GetChild(1).gameObject;

            var btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            // 버튼 리스너 없이 아래 OnPointerClick에서 직접 컨트롤
        }
        else
        {
            // 편지 아니면 클릭 이벤트 비움
            var btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
        }

        
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

            if (item.itemName == "Letter")
            {
                var letter = GetComponent<LetterItemBehavior>();
                if (!isAlreadySelected)
                {
                    letter.SetActiveLetterPanel(true);   // 퍼스트 셀렉트: 켜기
                }
                else
                {
                    letter.SetActiveLetterPanel(false);  // 세컨드 셀렉트: 끄기
                }
            }
            else if (item.itemName == "Note")
            {
                var note = GetComponent<NoteItemBehavior>();
                if (!isAlreadySelected)
                {
                    note.SetActiveNotePanel(true);   // 퍼스트 셀렉트: 켜기
                }
                else
                {
                    note.SetActiveNotePanel(false);  // 세컨드 셀렉트: 끄기
                }
            }

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
