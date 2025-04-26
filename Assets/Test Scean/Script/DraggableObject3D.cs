using UnityEngine;
using System.Collections;

public class DraggableObject3D : MonoBehaviour
{
    [Header("✅ 한 번 올리면 고정할지 여부")]
    [Tooltip("true로 설정 시 퍼즐 타일 위에 올려진 후 다시 움직일 수 없습니다.")]
    public bool lockOnDrop = false;

    private Vector3 offset;
    private float zCoord;
    private Vector3 originalPosition;
    private bool isLocked = false;

    void OnMouseDown()
    {
        // 잠금 설정이 활성화된 경우만 isLocked 체크
        if (lockOnDrop && isLocked) return;

        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        offset = transform.position - Camera.main.ScreenToWorldPoint(mousePoint);
        originalPosition = transform.position;

        Debug.Log("🖱️ 드래그 시작");
    }

    void OnMouseDrag()
    {
        if (lockOnDrop && isLocked) return;

        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePoint) + offset;
        transform.position = targetPos;
    }

    void OnMouseUp()
    {
        float detectionRadius = 1.0f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        bool snapped = false;

        foreach (Collider hit in hitColliders)
        {
            PuzzleTile tile = hit.GetComponent<PuzzleTile>();
            if (tile != null)
            {
                Vector3 snapPos = tile.transform.position;
                snapPos.z = tile.transform.position.z;

                StartCoroutine(SmoothMove(transform.position, snapPos, 0.2f));

                // 퍼즐 설정에 따라 고정 여부 결정
                if (lockOnDrop) isLocked = true;

                snapped = true;
                break;
            }
        }

        if (!snapped)
        {
            Debug.Log("🔁 퍼즐 타일 감지 안 됨 → 원위치 복귀");
            StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
        }
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

    /// <summary>
    /// 외부에서 강제로 리셋하고 싶을 때 호출
    /// </summary>
    public void ResetPosition()
    {
        isLocked = false;
        StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
    }
}

