using UnityEngine;

public class BookSlot : MonoBehaviour
{
    public string correctBookName;
    public bool isOccupied = false;
    public bool isCorrect = false;

    private GameObject currentBook;

    public bool TryInsertBook(GameObject book)
    {
        DraggableBook3D draggable = book.GetComponent<DraggableBook3D>();
        if (draggable == null) return false;

        // 슬롯이 이미 점유된 경우: 새 책은 넣지 못하게 함
        if (isOccupied)
        {
            Debug.Log("📕 슬롯이 이미 사용 중입니다!");
            return false;
        }

        currentBook = book;

        // ❌ 즉시 위치 덮어쓰기 제거 (책의 움직임은 DraggableBook3D에서 처리함)
        // book.transform.position = transform.position;

        isOccupied = true;
        isCorrect = (draggable.bookName == correctBookName);

        SlotManager slotManager = FindObjectOfType<SlotManager>();
        if (slotManager != null) slotManager.CheckSlotsNow();

        return true;
    }
}
