using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [Header("🎬 페이드 관련 UI")]
    public CanvasGroup fadeGroup;              // 화면 어둡게 처리할 CanvasGroup
    public TextMeshProUGUI failText;           // 실패 메시지 출력용 Text
    public float fadeDuration = 1f;            // 어두워지는 시간
    public float messageDuration = 2f;         // 메시지를 보여주는 시간

    /// <summary>
    /// 페이드 아웃 후 씬을 재시작 (대사 없이 바로 전환)
    /// </summary>
    public void FadeOutAndRestart()
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        if (fadeGroup != null && !fadeGroup.gameObject.activeSelf)
            fadeGroup.gameObject.SetActive(true);

        StartCoroutine(FadeAndReload());
    }

    /// <summary>
    /// 페이드 아웃 후 대사 출력 → 일정 시간 대기 → 씬 재시작
    /// </summary>
    public void ShowFailureDialogueThenRestart(string message)
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        if (fadeGroup != null && !fadeGroup.gameObject.activeSelf)
            fadeGroup.gameObject.SetActive(true);

        StartCoroutine(FadeAndShowMessage(message));
    }

    /// <summary>
    /// 단순 페이드 아웃 + 씬 재시작
    /// </summary>
    private IEnumerator FadeAndReload()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeGroup.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 페이드 아웃 → 텍스트 표시 → 대기 → 씬 재시작
    /// </summary>
    private IEnumerator FadeAndShowMessage(string message)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeGroup.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        if (failText != null)
        {
            failText.text = message;
            failText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(messageDuration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

