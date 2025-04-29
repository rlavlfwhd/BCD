using UnityEngine;
using UnityEngine.SceneManagement;

public class HoneyClickHandler : MonoBehaviour
{
    [Tooltip("클릭하면 이동할 씬 이름입니다.")]
    public string nextSceneName;

    private void OnMouseDown()
    {
        Debug.Log("🍯 꿀 클릭됨! 씬 전환합니다.");

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ 이동할 씬 이름이 설정되지 않았습니다!");
        }
    }
}
