using UnityEngine;

public class DraggableObject3D : MonoBehaviour
{
    private Vector3 offset;              // 마우스와 오브젝트 사이의 거리
    private float zCoord;               // z축 깊이 (마우스 → 월드 변환 시 필요)
    private Vector3 originalPosition;   // 실패 시 돌아갈 원래 위치
    private bool isLocked = false;      // 드랍 고정 여부

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

            if (hit.CompareTag("DropSlot"))
            {
                Debug.Log("✅ 태그 일치: DropSlot");

                BookSlot slot = hit.GetComponent<BookSlot>();
                if (slot != null)
                {
                    if (!slot.isOccupied)
                    {
                        Debug.Log("📚 BookSlot 있음 & 비어 있음 → 드랍 성공");

                        // 슬롯 위치 기준으로 살짝 앞으로 이동 (덮힘 방지)
                        Vector3 slotPosition = hit.transform.position;
                        slotPosition.z -= 0.1f; // 슬롯보다 앞쪽으로

                        StartCoroutine(SmoothMove(transform.position, slotPosition, 0.2f));

                        slot.isOccupied = true;
                        isLocked = true;

                        // ✅ 렌더 순서 조정 (책이 슬롯보다 앞에 보이도록)
                        Renderer r = GetComponent<Renderer>();
                        r.sortingLayerName = "Default"; // 필요 시 커스텀 이름 사용
                        r.sortingOrder = 10;            // 숫자 높을수록 위에 렌더링

                        // 🔧 머티리얼 큐도 조정 (Shader가 투명한 경우 보장용)
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
                    Debug.Log("🚫 BookSlot 컴포넌트 없음 (스크립트 누락)");
                }
            }
            else
            {
                Debug.Log("⛔ 태그 불일치: DropSlot 아님");
            }
        }

        Debug.Log("❌ 슬롯 없음 또는 실패 → 원위치로 복귀");
        StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
    }

    // 부드럽게 이동하는 코루틴
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
