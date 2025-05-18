using UnityEngine;

public enum SlotType { Book, Statue }

public class BookSlot : MonoBehaviour
{
    public SlotType slotType = SlotType.Book;
    public string correctBookName;

    public bool isOccupied = false;
    public bool isCorrect = false;

    private GameObject currentItem;

    public bool TryInsertBook(GameObject item)
    {
        DraggableItem draggable = item.GetComponent<DraggableItem>();
        if (draggable == null)
        {
            Debug.Log("❌ 드래그 가능한 아이템이 아닙니다.");
            return false;
        }

        // 🧠 슬롯과 아이템 타입이 일치하지 않으면 거부
        if ((slotType == SlotType.Book && draggable.itemType != ItemType.Book) ||
            (slotType == SlotType.Statue && draggable.itemType != ItemType.Statue))
        {
            Debug.Log("🚫 슬롯 타입과 아이템 타입이 일치하지 않습니다!");
            return false;
        }

        if (isOccupied)
        {
            Debug.Log("📦 슬롯이 이미 사용 중입니다!");
            return false;
        }

        currentItem = item;
        isOccupied = true;
        isCorrect = (draggable.bookName == correctBookName);

        SlotManager slotManager = FindObjectOfType<SlotManager>();
        if (slotManager != null) slotManager.CheckSlotsNow();

        return true;
    }
}