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

        // 기존 책 제거
        if (isOccupied && currentBook != null)
        {
            DraggableBook3D previous = currentBook.GetComponent<DraggableBook3D>();
            if (previous != null) previous.ReturnToOriginalPosition();
        }

        currentBook = book;
        book.transform.position = transform.position;
        isOccupied = true;

        isCorrect = (draggable.bookName == correctBookName);

        SlotManager slotManager = FindObjectOfType<SlotManager>();
        if (slotManager != null) slotManager.CheckSlotsNow();

        return true;
    }
}
