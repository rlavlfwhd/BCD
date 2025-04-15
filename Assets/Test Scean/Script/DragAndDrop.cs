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

        Debug.Log("�巡�� ����");
    }

    void OnMouseDrag()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        transform.position = mouseWorldPos + offset;
    }

    void OnMouseUp()
    {
        Debug.Log("�巡�� ����");

        // ���� ���� �ȿ� �ִ��� Ȯ�� (2D �浹 �˻�)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("DropSlot"))
            {
                transform.position = hit.transform.position;
                Debug.Log("���Կ� �����");
                return;
            }
        }

        Debug.Log("������ �ƴ�. ���� ��ġ�� ������");
    }
}
