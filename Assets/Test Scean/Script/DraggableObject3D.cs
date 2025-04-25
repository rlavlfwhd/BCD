using UnityEngine;

public class DraggableObject3D : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private Vector3 originalPosition;
    private bool isLocked = false;
  
    void OnMouseDown()
    {
        if (isLocked) return;

        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePoint);
        originalPosition = transform.position;

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
        Debug.Log($"🔍 감지된 콜라이더 수: {hitColliders.Length}");

        foreach (Collider hit in hitColliders)
        {
            Debug.Log($"➡️ 감지된 오브젝트 이름: {hit.name}");

            // 🎯 BookSlot 처리
            if (hit.CompareTag("DropSlot"))
            {
                Debug.Log("태그 일치: DropSlot");

                BookSlot slot = hit.GetComponent<BookSlot>();
                if (slot != null)
                {
                    if (!slot.isOccupied)
                    {
                        Debug.Log("📚 BookSlot 있음 & 비어 있음 → 드랍 성공");

                        Vector3 slotPosition = hit.transform.position;
                        slotPosition.z -= 0.1f;

                        StartCoroutine(SmoothMove(transform.position, slotPosition, 0.2f));

                        slot.isOccupied = true;
                        isLocked = true;

                        Renderer r = GetComponent<Renderer>();
                        r.sortingLayerName = "Default";
                        r.sortingOrder = 10;
                        r.material.renderQueue = 2501;

                        return;
                    }
                    else
                    {
                        Debug.Log("⚠️ BookSlot 있음 BUT 이미 사용 중");
                    }
                }
                else
                {
                    Debug.Log("🚫 BookSlot 컴포넌트 없음");
                }
            }
        }        
        StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
    }

    System.Collections.IEnumerator SmoothMove(Vector3 fromPos, Vector3 toPos, float duration)
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
