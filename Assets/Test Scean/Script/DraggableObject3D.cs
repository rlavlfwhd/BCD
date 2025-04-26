using System.Collections;
using UnityEngine;

/// <summary>
/// 3D 작물을 드래그하고,
/// 퍼즐 타일 감지는 CropDetector가 담당.
/// 감지가 성공했든 실패했든 그냥 드래그/복귀만 관리.
/// </summary>
public class DraggableObject3D : MonoBehaviour
{
    private Vector3 offset;           // 드래그할 때 마우스와 오브젝트 차이
    private float zCoord;             // 드래그 시 유지할 Z값
    private Vector3 originalPosition; // 원래 위치 저장

    [Tooltip("드래그 실패 시 복귀할 위치 (없으면 원래 위치)")]
    public Transform fallbackPosition;

    [Tooltip("퍼즐 타일 위에 올려도 고정하지 않고 계속 드래그할 수 있는지 여부")]
    public bool lockOnDrop = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePoint);

        Debug.Log("🖱️ 드래그 시작");
    }

    void OnMouseDrag()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePoint) + offset;
        transform.position = targetPos;
    }

    void OnMouseUp()
    {
        Debug.Log("🖱️ 드래그 종료");

        // 드랍했을 때 현재 위치 주변 퍼즐 타일 탐색
        float detectionRadius = 1.0f;
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("PlaceSlot"))
            {
                Debug.Log($"🟩 퍼즐 타일에 드롭 감지됨: {hit.name}");

                // 퍼즐 타일 중앙에 스냅 이동
                Vector3 snapPos = hit.transform.position;
                snapPos.z -= 0.1f;
                StartCoroutine(SmoothMove(transform.position, snapPos, 0.2f));

                if (!lockOnDrop)
                {
                    Debug.Log("🔁 고정 없이 계속 이동 가능");
                }
                return;
            }
        }

        // 주변에 퍼즐 타일이 없으면 → fallback 위치나 원래 위치로 복귀
        Debug.Log("⛔ 퍼즐 타일 감지 실패, 원위치 복귀");
        Vector3 returnPos = fallbackPosition != null ? fallbackPosition.position : originalPosition;
        StartCoroutine(SmoothMove(transform.position, returnPos, 0.2f));
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



