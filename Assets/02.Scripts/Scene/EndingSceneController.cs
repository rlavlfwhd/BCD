using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneController : MonoBehaviour
{
    public string mainSceneName = "MainScene";

    private void Start()
    {
        // 챕터 연출이 끝나면 실행될 콜백 등록
        if (ChapterController.Instance != null)
        {
            ChapterController.Instance.OnChapterFinished = () =>
            {
                StartCoroutine(GoToMainSceneAfterDelay());
            };
        }
    }

    private IEnumerator GoToMainSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f); // 챕터 종료 후 2초 대기

        SceneFadeController fadeController = FindObjectOfType<SceneFadeController>();
        if (fadeController != null)
        {
            fadeController.FadeAndLoadScene(mainSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainSceneName);
        }
    }
}