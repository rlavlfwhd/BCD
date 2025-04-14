using UnityEngine;

public class BookcaseClickHandler : MonoBehaviour
{
    public GameObject layer0;                  // 0Layer ������Ʈ
    public GameObject layer1;                  // 1Layer ������Ʈ
    public GameObject[] objectsToHide;         // Ŭ�� �� ���� �ٸ� ������Ʈ��
    public GameObject[] bookObjects;           // å ������Ʈ�� (��Ȱ��ȭ ���·� ����)

    void OnMouseDown()
    {
        if (layer0 != null) layer0.SetActive(false);
        if (layer1 != null) layer1.SetActive(true);

        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null) obj.SetActive(false);
        }

        Debug.Log("å�� Ŭ�� �� 1Layer ��� Ȱ��ȭ + ���� ������Ʈ ���� + å ������Ʈ Ȱ��ȭ �Ϸ�!");
    }
}
