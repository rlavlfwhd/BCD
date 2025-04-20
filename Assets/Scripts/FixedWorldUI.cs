using UnityEngine;

public class FixedWorldUI : MonoBehaviour
{
    private Transform cam; // ī�޶� ����
    private Vector3 initialOffset; // �ʱ� UI ��ġ ������

    void Start()
    {
        cam = Camera.main.transform; // ���� ī�޶� ã��
        initialOffset = transform.position - cam.position; // ó�� �Ÿ� ����
    }

    void LateUpdate()
    {
        // ī�޶� ��ġ�� �������� ȸ���� ����
        transform.position = cam.position + initialOffset;
        transform.rotation = Quaternion.identity; // ȸ�� ����
    }
}