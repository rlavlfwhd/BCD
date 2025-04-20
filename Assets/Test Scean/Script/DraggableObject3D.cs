using UnityEngine;

public class DraggableObject3D : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private Vector3 originalPosition;
    private bool isLocked = false;

    [Header("Mirror 연출용")]
    public GameObject mirrorOriginal;     // 원래 거울
    public GameObject mirrorAlternate;    // 바뀐 거울
    public GameObject pendantObject;      // 펜던트 오브젝트 (Mirror2가 나타날 때 활성화)

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
                Debug.Log("✅ 태그 일치: DropSlot");

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

            // 🪞 Mirror 처리
            else if (hit.CompareTag("Mirror"))
            {
                Debug.Log("🪞 거울 감지됨 → 연출 시작");

                if (mirrorOriginal != null) mirrorOriginal.SetActive(false);
                if (mirrorAlternate != null) mirrorAlternate.SetActive(true);
                if (pendantObject != null) pendantObject.SetActive(true); // ✅ 펜던트도 활성화

                isLocked = true;

                gameObject.SetActive(false); // 🎯 이 오브젝트를 비활성화

                Debug.Log("✅ 거울 교체 + 펜던트 활성화 + 오브젝트 비활성화 완료");
                return;
            }

            // 🚪 Door 처리 (펜던트를 드랍하면 문과 펜던트 모두 사라짐)
            else if (hit.CompareTag("Door"))
            {
                Debug.Log("🚪 Door 감지됨 → 문과 펜던트 비활성화 시도");

                if (hit.gameObject != null)
                {
                    hit.gameObject.SetActive(false);   // 문 비활성화
                    gameObject.SetActive(false);       // 펜던트 자신 비활성화
                    isLocked = true;

                    Debug.Log("✅ 문과 펜던트 비활성화 완료");
                    return;
                }
                else
                {
                    Debug.LogWarning("❌ 감지된 Door 오브젝트가 null임!");
                }
            }

            else
            {
                Debug.Log("⛔ 태그 불일치: DropSlot/Mirror/Door 아님");
            }
        }

        Debug.Log("❌ 슬롯 없음 또는 실패 → 원위치로 복귀");
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
