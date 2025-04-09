using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float waitBeforeFadeOut = 2f;

    void Start()
    {
        canvasGroup.alpha = 0f; // 시작할 때 완전히 투명하게
        StartCoroutine(FadeInOut());
    }

    System.Collections.IEnumerator FadeInOut()
    {
        // 페이드 인
        yield return StartCoroutine(Fade(0f, 1f));

        // 페이드 인 완료 후 대기
        yield return new WaitForSeconds(waitBeforeFadeOut);

        // 페이드 아웃
        yield return StartCoroutine(Fade(1f, 0f));

        // 완전히 사라지면 비활성화
        canvasGroup.gameObject.SetActive(false);
    }

    System.Collections.IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
