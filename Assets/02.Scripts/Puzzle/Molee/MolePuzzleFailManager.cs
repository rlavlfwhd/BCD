using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MolePuzzleFailManager : MonoBehaviour
{
    public static MolePuzzleFailManager Instance;
    
    public int nextStoryIndex = 114;
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
        StartCoroutine(GoToStoryAfterDelay());
    }

    private IEnumerator GoToStoryAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));
    }

    public void HandleFail()
    {
        ChapterController.skipChapterOnLoad = true;

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
