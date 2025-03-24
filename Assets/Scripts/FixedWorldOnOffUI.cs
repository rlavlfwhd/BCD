using UnityEngine;

public class FixedWorldOnOffUI : MonoBehaviour
{
    private Transform cam; // ī�޶� ����
    private Vector3 initialOffset; // �ʱ� UI ��ġ ������
    public float distanceFromCamera = 5f; // ī�޶󿡼� �󸶳� ��������

    void Start()
    {
        cam = Camera.main.transform; // ���� ī�޶� ã��
        SetUIPosition(); // ������ �� �� �� ��ġ ����
    }

    void OnEnable()
    {
        SetUIPosition(); // UI�� Ȱ��ȭ�� �� ��ġ ����
    }

    void SetUIPosition()
    {
        if (cam == null) return;

        // ī�޶� ����(distanceFromCamera��ŭ ������ ��)�� ��ġ
        transform.position = cam.position + cam.forward * distanceFromCamera;

        // UI�� ī�޶� �������� �ٶ󺸰� ����
        transform.rotation = cam.rotation;
    }

    void LateUpdate()
    {
        // UI�� ���� �Ŀ��� ī�޶� ��ġ�� ���󰡵��� ����
        transform.position = cam.position + cam.forward * distanceFromCamera;
        transform.rotation = cam.rotation;
    }
}
