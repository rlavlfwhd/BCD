using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    public CanvasGroup fadeCanvas; // 페이드 효과를 위한 CanvasGroup
    public float fadeDuration = 1.5f; // 페이드 인 지속 시간

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fadeCanvas.alpha = 1; // 처음엔 완전히 검은 화면
        while (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        fadeCanvas.interactable = false;
        fadeCanvas.blocksRaycasts = false; // UI 클릭 방지 해제
    }
}
