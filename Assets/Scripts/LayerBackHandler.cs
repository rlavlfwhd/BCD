using UnityEngine;

public class LayerBackHandler : MonoBehaviour
{
    public GameObject layer0; // �ٽ� ���� 0Layer
    public GameObject layer1; // �ٽ� ���� 1Layer
    public GameObject[] objectsToShow; // �ٽ� ������ ������Ʈ��

    void OnMouseDown()
    {
        if (layer1 != null) layer1.SetActive(false);
        if (layer0 != null) layer0.SetActive(true);

        foreach (GameObject obj in objectsToShow)
        {
            obj.SetActive(true);
        }

        Debug.Log("�̵� ������Ʈ Ŭ�� �� 0Layer ���� + ���� ������Ʈ �ٽ� ǥ�� �Ϸ�!");
    }
}
