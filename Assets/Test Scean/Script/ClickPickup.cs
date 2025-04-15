using UnityEngine;
using TMPro; // TextMeshPro ����� ���� ���ӽ����̽�

public class ClickPickup : MonoBehaviour
{
    public GameObject pickupUIPanel; // �ؽ�Ʈ â ��ü �г� ������Ʈ
    public TextMeshProUGUI pickupUIText; // �г� �ȿ� �� �ؽ�Ʈ (TextMeshProUGUI)

    private Camera mainCamera; // ī�޶� ����
    public float hideDelay = 2f; // �ؽ�Ʈ â�� ����� �ð� (��)

    void Start()
    {
        mainCamera = Camera.main; // ���� ī�޶� ��������

        // ���� �� �ؽ�Ʈ UI â ��Ȱ��ȭ
        if (pickupUIPanel != null)
            pickupUIPanel.SetActive(false);
    }

    void Update()
    {
        // ���콺 ���� Ŭ�� ����
        if (Input.GetMouseButtonDown(0))
        {
            // Ŭ���� ��ġ�� ���� ��ǥ�� ��ȯ
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // �ش� ��ġ�� ����ĳ��Ʈ �߻�
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // ���𰡿� �ε����ٸ�
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                // �� ��ũ��Ʈ�� ���� ������Ʈ�� Ŭ���� ���
                if (clickedObject == gameObject)
                {
                    Debug.Log("������Ʈ Ŭ����: " + gameObject.name);

                    // ������Ʈ ��Ȱ��ȭ (�������)
                    gameObject.SetActive(false);

                    // �ؽ�Ʈ ����: ������Ʈ �̸� + "ȹ��"
                    pickupUIText.text = $"{gameObject.name} ȹ��";

                    // UI â ���̰� �ϱ�
                    pickupUIPanel.SetActive(true);

                    // ���� �ð� �� UI â ��Ȱ��ȭ
                    Invoke("HidePickupUI", hideDelay);
                }
            }
        }
    }

    // �ؽ�Ʈ UI â�� ����� �Լ�
    void HidePickupUI()
    {
        if (pickupUIPanel != null)
            pickupUIPanel.SetActive(false);
    }
}
