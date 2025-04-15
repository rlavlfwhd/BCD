using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();

        Debug.Log("드래그 시작");
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        Debug.Log("드래그 종료");

        // 주변에 슬롯이 있는지 확인
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("DropSlot"))
            {
                transform.position = hit.transform.position;
                Debug.Log("슬롯에 드랍됨");
                return;
            }
        }

        // 슬롯이 아니라면 현재 위치에 그대로 둠
        Debug.Log("슬롯 아님. 현재 위치에 고정됨");
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}