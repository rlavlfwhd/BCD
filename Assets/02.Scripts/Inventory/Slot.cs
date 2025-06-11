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

        if (item != null && item.itemName == "Note")
        {
            var note = gameObject.AddComponent<NoteItemBehavior>();
            note.noteViewRoot = GameObject.Find("NoteView");   // 씬 내 쪽지 뷰 오브젝트명
            note.darkFilterImage = note.noteViewRoot.transform.GetChild(0).gameObject; // 필요시

            var btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(note.OnClickNote);
        }

        if (item != null && item.itemName == "Letter")
        {
            var letter = gameObject.AddComponent<LetterItemBehavior>();
            letter.letterPaperRoot = GameObject.Find("LetterContents");
            GameObject[] objs = new GameObject[4];
            for (int i = 0; i < 4; i++)
                objs[i] = letter.letterPaperRoot.transform.GetChild(i + 1).gameObject;
            letter.letterPaperObjects = objs;

            // DarkFilterImage는 0번
            letter.darkFilterImage = letter.letterPaperRoot.transform.GetChild(0).gameObject;

            // --- 보석 아이템 배열 동적 할당 ---
            Item[] jewels = new Item[4];
            jewels[0] = Resources.Load<Item>("Items/Gem_Pink"); // 이름과 경로는 실제 프로젝트에 맞게!
            jewels[1] = Resources.Load<Item>("Items/Gem_Red");
            jewels[2] = Resources.Load<Item>("Items/Gem_Brown");
            jewels[3] = Resources.Load<Item>("Items/Gem_White");
            letter.jewelItems = jewels;

            // 버튼 OnClick 연결
            var btn = GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(letter.OnClickLetter);
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
