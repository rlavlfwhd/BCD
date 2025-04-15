using UnityEngine;

public class DraggableObject2D : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 mouseWorldPos;

    void OnMouseDown()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        offset = transform.position - mouseWorldPos;

        Debug.Log("드래그 시작");
    }

    void OnMouseDrag()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        transform.position = mouseWorldPos + offset;
    }

    void OnMouseUp()
    {
        Debug.Log("드래그 종료");

        // 슬롯 범위 안에 있는지 확인 (2D 충돌 검사)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("DropSlot"))
            {
                transform.position = hit.transform.position;
                Debug.Log("슬롯에 드랍됨");
                return;
            }
        }

        Debug.Log("슬롯이 아님. 현재 위치에 고정됨");
    }
}
