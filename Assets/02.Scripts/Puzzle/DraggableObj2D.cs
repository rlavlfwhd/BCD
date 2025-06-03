using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DraggableObj2D : MonoBehaviour
{
    protected Vector3 originalPosition;
    [Tooltip("드래그 실패 시 복귀할 위치 (없으면 원래 위치)")]
    public Transform fallbackPosition;

    protected virtual void Start()
    {
        originalPosition = transform.position;
    }

    public virtual void OnDragStart() { }
    public virtual void OnDragMove(Vector3 newPos)
    {
        newPos.z = transform.position.z;
        transform.position = newPos;
    }

    public virtual void OnDragEnd()
    {
        // 각 퍼즐 타입별 체크는 자식에서 구현
    }

    public void SnapToSlot(Vector3 slotPosition)
    {
        StopAllCoroutines(); // 이전 이동 중이면 취소
        StartCoroutine(SmoothMove(transform.position, slotPosition, 0.2f));
        originalPosition = slotPosition;
    }

    protected void ReturnToOriginalPosition()
    {
        Vector3 returnPos = fallbackPosition != null ? fallbackPosition.position : originalPosition;
        StartCoroutine(SmoothMove(transform.position, returnPos, 0.2f));
    }

    protected IEnumerator SmoothMove(Vector3 fromPos, Vector3 toPos, float duration)
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
