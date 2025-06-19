using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("🎬 이동할 씬 이름 입력")]
    public string sceneName = "PFlowerScene";

    // 마우스로 클릭했을 때
    private void OnMouseDown()
    {
        StartCoroutine(FadeManager.Instance.FadeToChoiceScene(sceneName));
    }
}
