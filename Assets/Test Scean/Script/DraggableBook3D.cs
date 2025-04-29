using UnityEngine;
using System.Collections;

public class DraggableBook3D : MonoBehaviour
{
    [Header("이 책의 고유 이름 (예: BlueBook, RedBook 등)")]
    public string bookName;

    private Vector3 offset;
    private float zCoord;
    private Vector3 originalPosition;
    private bool isLocked = false;

    private BookSlot currentSlot = null;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (isLocked) return;

        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePoint);

        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }

        Debug.Log("🖱️ 드래그 시작 (3D)");
    }

    void OnMouseDrag()
    {
        if (isLocked) return;

        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePoint) + offset;
        transform.position = targetPos;
    }

    void OnMouseUp()
    {
        if (isLocked) return;

        Debug.Log("🖱️ 드래그 종료 (3D)");

        float detectionRadius = 1.0f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("DropSlot"))
            {
                BookSlot slot = hit.GetComponent<BookSlot>();
                if (slot != null)
                {
                    bool success = slot.TryInsertBook(gameObject);

                    if (success)
                    {
                        Vector3 slotPosition = hit.transform.position;
                        slotPosition.z -= 0.1f;

                        StartCoroutine(SmoothMove(transform.position, slotPosition, 0.2f));

                        currentSlot = slot;
                        isLocked = false;

                        Renderer r = GetComponent<Renderer>();
                        if (r != null)
                        {
                            r.sortingLayerName = "Default";
                            r.sortingOrder = 10;
                            r.material.renderQueue = 2501;
                        }

                        return;
                    }
                }
            }
        }
        // 슬롯 못 찾았을 때 아무 것도 안 함
    }

    public void ReturnToOriginalPosition()
    {
        StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }
    }

    public void ForceRemoveFromSlot()
    {
        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }

        isLocked = false;
        StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
    }

    public void ForceSwapToPosition(Vector3 targetPosition)
    {
        StartCoroutine(SmoothMove(transform.position, targetPosition, 0.2f));
        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }
    }

    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }

    IEnumerator SmoothMove(Vector3 fromPos, Vector3 toPos, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(fromPos, toPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = toPos;
    }
}
