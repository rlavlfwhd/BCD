using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   

    // ���콺�� Ŭ������ ��
    private void OnMouseDown()
    {

        StartCoroutine(FadeManager.Instance.FadeToChoiceScene("PFlowerScene"));
    }
}
