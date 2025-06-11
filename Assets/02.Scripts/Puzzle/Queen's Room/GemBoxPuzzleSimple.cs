using UnityEngine;
using UnityEngine.UI;

public class GemBoxPuzzleManager : MonoBehaviour
{
    public GemSlotSimple[] slots;
    public GameObject boxClosed;
    public GameObject boxOpen;

    public Item[] gemItems; // 인벤토리에서 사용하는 보석 4개
    public Item noteItem;   // 인벤토리로 지급할 쪽지
    public Item letterItem; // 인벤토리에서 제거할 편지

    private void Start()
    {
        boxClosed.SetActive(true);
        boxOpen.SetActive(false);

        foreach (var slot in slots)
        {
            slot.currentGemName = "";
            slot.UpdateSlotUI(null);
        }
    }

    // 인벤토리 보석 클릭 후 슬롯 클릭시 호출
    public void TryPlaceGemToSlot(GemSlotSimple slot)
    {
        Debug.Log("[GemBoxPuzzle] TryPlaceGemToSlot 호출! 슬롯:" + slot.name);

        var selectedItem = Inventory.Instance.firstSelectedItem;
        if (selectedItem == null)
        {
            Debug.Log("[GemBoxPuzzle] 선택된 아이템이 없음");
            return;
        }

        int idx = System.Array.IndexOf(gemItems, selectedItem);
        if (idx == -1)
        {
            Debug.Log("[GemBoxPuzzle] 선택된 아이템이 보석아이템이 아님: " + selectedItem.itemName);
            return;
        }

        if (!string.IsNullOrEmpty(slot.currentGemName))
        {
            Debug.Log("[GemBoxPuzzle] 이미 꽂힌 슬롯임:" + slot.name);
            return;
        }

        slot.currentGemName = selectedItem.itemName;
        slot.UpdateSlotUI(selectedItem.itemImage);

        Debug.Log("[GemBoxPuzzle] 슬롯에 보석 장착 성공:" + selectedItem.itemName);

        Inventory.Instance.RemoveItem(selectedItem);
        Inventory.Instance.ClearSelection();

        if (AreAllSlotsFilled())
            CheckAnswer();
    }

    bool AreAllSlotsFilled()
    {
        foreach (var slot in slots)
            if (string.IsNullOrEmpty(slot.currentGemName))
                return false;
        return true;
    }

    void CheckAnswer()
    {
        bool correct = true;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentGemName != slots[i].correctGemName)
            {
                correct = false;
                break;
            }
        }

        if (correct)
            OnPuzzleClear();
        else
            ResetPuzzle();
    }

    void OnPuzzleClear()
    {
        boxClosed.SetActive(false);
        boxOpen.SetActive(true);

        Inventory.Instance.RemoveItem(letterItem);
    }

    void ResetPuzzle()
    {
        foreach (var slot in slots)
        {
            if (!string.IsNullOrEmpty(slot.currentGemName))
            {
                var item = System.Array.Find(gemItems, x => x.itemName == slot.currentGemName);
                if (item != null)
                    Inventory.Instance.AddItem(item);
            }
            slot.currentGemName = "";
            slot.UpdateSlotUI(null);
        }
    }
}
