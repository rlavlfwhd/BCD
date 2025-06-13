using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   

    // 마우스로 클릭했을 때
    private void OnMouseDown()
    {

        StartCoroutine(FadeManager.Instance.FadeToChoiceScene("PFlowerScene"));
    }
}
