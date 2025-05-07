using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float waitBeforeFadeOut = 2f;

    private Coroutine fadeCoroutine;

    void Start()
    {
        canvasGroup.alpha = 0f; // 시작할 때 완전히 투명하게
        Play(); // 자동 실행
    }

    public void Play()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;

        // 🔥 이 두 줄 꼭 필요!
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        fadeCoroutine = StartCoroutine(FadeInOut());
    }

    System.Collections.IEnumerator FadeInOut()
    {
        yield return StartCoroutine(Fade(0f, 1f));
        yield return new WaitForSeconds(waitBeforeFadeOut);
        yield return StartCoroutine(Fade(1f, 0f));

        // 🔥 페이드 아웃 후 UI 상호작용 막기
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

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
