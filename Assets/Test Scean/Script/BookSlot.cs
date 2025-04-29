using UnityEngine;

public class BookSlot : MonoBehaviour
{
    public string correctBookName;
    public bool isOccupied = false;
    public bool isCorrect = false;
    

    private GameObject currentBook;

    public bool TryInsertBook(GameObject book)
    {
        DraggableBook3D newBookComponent = book.GetComponent<DraggableBook3D>();
        if (newBookComponent == null) return false;

        if (isOccupied && currentBook != null)
        {
            // 슬롯에 이미 책이 있으면 자리 교체
            DraggableBook3D oldBookComponent = currentBook.GetComponent<DraggableBook3D>();
            if (oldBookComponent != null)
            {
                oldBookComponent.ForceSwapToPosition(newBookComponent.GetOriginalPosition());
            }
        }

        // 슬롯에 새 책 넣기
        currentBook = book;

        // 슬롯 중앙에 새 책 배치
        book.transform.position = transform.position;
        book.transform.SetParent(transform);

        isOccupied = true;
        isCorrect = (newBookComponent.bookName == correctBookName);

        Debug.Log(isCorrect ? "정답 책 들어감!" : "틀린 책 들어감!");

        SlotManager manager = FindObjectOfType<SlotManager>();
        if (manager != null)
        {
            manager.CheckSlotsNow();
        }

        return true;
    }
}
