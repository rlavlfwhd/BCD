using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DraggableObj2D : MonoBehaviour
{
    protected Vector3 originalPosition;
    [Tooltip("�巡�� ���� �� ������ ��ġ (������ ���� ��ġ)")]
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
        // �� ���� Ÿ�Ժ� üũ�� �ڽĿ��� ����
    }

    public void SnapToSlot(Vector3 slotPosition)
    {
        StopAllCoroutines(); // ���� �̵� ���̸� ���
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
