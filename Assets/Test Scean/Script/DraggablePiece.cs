using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggablePiece : MonoBehaviour
{
    public GameObject targetObject;      // 변환될 위쪽 조각
    public Transform defaultPosition;    // 원래 위치 저장용
    private bool isDragging = false;
    private Vector3 offset;

    void OnMouseDown()
    {
        if (!targetObject.activeSelf)
        {
            // 변환 전 상태: 교체
            targetObject.SetActive(true);
            gameObject.SetActive(false);
            return;
        }

        // 변환 후 상태: 드래그 시작
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPos() + offset;
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;

            // 슬롯 감지 시 처리
            GameObject slot = FindNearestSlot();
            if (slot != null)
            {
                transform.position = slot.transform.position;
                Debug.Log("✅ 슬롯에 성공적으로 드랍됨!");
            }
            else
            {
                // 슬롯 못 찾으면 원래 자리로 되돌림
                if (defaultPosition != null)
                {
                    transform.position = defaultPosition.position;
                    Debug.Log("🔁 슬롯이 없어 원래 위치로 복귀함");
                }
            }
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0; // 2D 게임이니까 z 고정
        return mousePoint;
    }

    GameObject FindNearestSlot()
    {
        float radius = 1f; // 슬롯 감지 반경
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Slot")) // 슬롯 오브젝트에는 Slot 태그 부여
            {
                return hit.gameObject;
            }
        }
        return null;
    }
}

