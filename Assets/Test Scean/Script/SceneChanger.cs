using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // ��ȯ�� �� �̸�
    [SerializeField] private string sceneToLoad = "GardenScene";

    // ���콺�� Ŭ������ ��
    private void OnMouseDown()
    {
        // �� ��ȯ
        SceneManager.LoadScene(sceneToLoad);
    }
}
