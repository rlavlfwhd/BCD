using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
    public BookPuzzleManager puzzleManager;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableBook book = eventData.pointerDrag.GetComponent<DraggableBook>();
        if (book != null)
        {
            book.transform.SetParent(transform);
            puzzleManager.RegisterBook(book.bookColor);
        }
    }
}
