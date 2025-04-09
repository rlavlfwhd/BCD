using UnityEngine;

public class FadeUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;
    public float waitBeforeFadeOut = 2f;

    void Start()
    {
        canvasGroup.alpha = 0f; // ������ �� ������ �����ϰ�
        StartCoroutine(FadeInOut());
    }

    System.Collections.IEnumerator FadeInOut()
    {
        // ���̵� ��
        yield return StartCoroutine(Fade(0f, 1f));

        // ���̵� �� �Ϸ� �� ���
        yield return new WaitForSeconds(waitBeforeFadeOut);

        // ���̵� �ƿ�
        yield return StartCoroutine(Fade(1f, 0f));

        // ������ ������� ��Ȱ��ȭ
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
