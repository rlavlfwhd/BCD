using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();

        Debug.Log("�巡�� ����");
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        Debug.Log("�巡�� ����");

        // �ֺ��� ������ �ִ��� Ȯ��
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("DropSlot"))
            {
                transform.position = hit.transform.position;
                Debug.Log("���Կ� �����");
                return;
            }
        }

        // ������ �ƴ϶�� ���� ��ġ�� �״�� ��
        Debug.Log("���� �ƴ�. ���� ��ġ�� ������");
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}