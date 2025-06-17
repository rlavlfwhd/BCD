using UnityEngine;
using UnityEngine.SceneManagement;

public class MolePuzzleFailManager : MonoBehaviour
{
    public static MolePuzzleFailManager Instance;

    public string nextSceneName = "StoryScene";
    public int nextStoryIndex = -1;
    public FadeController fadeController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void HandleSuccess()
    {
        if (nextStoryIndex >= 0)
        {
            SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("시름 이동 정보가 비어있습니다.");
        }
    }

    public void HandleFail()
    {
        if (fadeController != null)
        {
            fadeController.FadeOutAndRestart();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
