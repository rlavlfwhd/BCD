using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSceneController : MonoBehaviour
{
    public string mainSceneName = "MainScene";

    private void Start()
    {
        // é�� ������ ������ ����� �ݹ� ���
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
        yield return new WaitForSeconds(2f); // é�� ���� �� 2�� ���

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