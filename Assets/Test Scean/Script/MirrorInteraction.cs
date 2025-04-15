using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{
    // Layer1 ������Ʈ �׷� (�⺻ ȭ��)
    public GameObject layer1Objects;

    // Layer2 ������Ʈ �׷� (�ſ� Ŭ�� �� ��ȯ�Ǵ� ȭ��)
    public GameObject layer2Objects;

    // ���� ���¸� �����ϴ� ����
    private bool isInLayer2 = false;

    void OnMouseDown()
    {
        // ���� ���°� Layer2���� Ȯ��
        if (!isInLayer2)
        {
            // Layer1 ��Ȱ��ȭ
            if (layer1Objects != null)
                layer1Objects.SetActive(false);

            // Layer2 Ȱ��ȭ
            if (layer2Objects != null)
                layer2Objects.SetActive(true);

            Debug.Log("�ſ� Ŭ��: Layer2�� ��ȯ��");
        }
        else
        {
            // Layer1 �ٽ� Ȱ��ȭ
            if (layer1Objects != null)
                layer1Objects.SetActive(true);

            // Layer2 ��Ȱ��ȭ
            if (layer2Objects != null)
                layer2Objects.SetActive(false);

            Debug.Log("�ſ� Ŭ��: Layer1�� ���ư�");
        }

        // ���� ����
        isInLayer2 = !isInLayer2;
    }
}
