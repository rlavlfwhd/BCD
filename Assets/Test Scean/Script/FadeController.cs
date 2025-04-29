using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [Header("🎬 페이드 UI")]
    public CanvasGroup fadeGroup;        // 페이드용 CanvasGroup 연결
    public float fadeDuration = 1f;      // 페이드 아웃 속도 (초 단위)

    /// <summary>
    /// 페이드 아웃 후 현재 씬을 다시 로드함 (퍼즐 실패 등에서 호출)
    /// </summary>
    public void FadeOutAndRestart()
    {
        StartCoroutine(FadeAndReload());
    }

    private IEnumerator FadeAndReload()
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeGroup.alpha = Mathf.Clamp01(t); // 0 → 1
            yield return null;
        }

        // 다 어두워졌으면 씬 다시 시작
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 페이드 인 효과 (원하면 시작 시 자동 밝아지기)
    /// </summary>
    public void FadeInOnStart()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime / fadeDuration;
            fadeGroup.alpha = Mathf.Clamp01(t); // 1 → 0
            yield return null;
        }
    }
}
