using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FadeController : MonoBehaviour
{
    [Header("🎬 페이드 이미지 패널 (F_Image)")]
    public GameObject fadePanel;
    public CanvasGroup fadeGroup;
    public TMP_Text failText;
    public string failMessage = "퍼즐 실패! 다시 도전하세요!";

    public float fadeDuration = 1.5f;
    public float messageDelay = 1f;

    private void Awake()
    {
        if (fadePanel != null)
        {
            fadePanel.SetActive(true);          // 페이드 인 위해 켜둠
        }

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 1f;               // 시작은 검게
        }

        if (failText != null)
        {
            failText.gameObject.SetActive(false);
        }

        StartCoroutine(FadeInFromBlack());
    }

    public void FadeOutAndRestart()
    {
        Debug.Log("✅ FadeOutAndRestart 호출됨");

        if (fadePanel != null && !fadePanel.activeInHierarchy)
        {
            fadePanel.SetActive(true);
        }

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
        }

        if (failText != null)
        {
            failText.gameObject.SetActive(false);
        }

        StartCoroutine(FadeAndShowText());
    }

    private IEnumerator FadeAndShowText()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            if (fadeGroup != null)
            {
                fadeGroup.alpha = Mathf.Clamp01(t);
            }
            yield return null;
        }

        Debug.Log("✅ 페이드 완료 → 텍스트 표시");

        if (failText != null)
        {
            failText.text = failMessage;
            failText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(messageDelay);

        Debug.Log("✅ 씬 다시 로드");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FadeInFromBlack()
    {
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            if (fadeGroup != null)
            {
                fadeGroup.alpha = Mathf.Clamp01(1f - t);
            }
            yield return null;
        }

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.interactable = false;
            fadeGroup.blocksRaycasts = false;
        }

        if (fadePanel != null)
        {
            fadePanel.SetActive(false);
        }

        Debug.Log("✅ 페이드 인 완료");
    }
}
