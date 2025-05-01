using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FadeController : MonoBehaviour
{
    [Header("🎬 페이드 이미지 패널 (F_Image)")]
    public GameObject fadePanel;       // F_Image 패널
    public CanvasGroup fadeGroup;      // F_Image 안의 CanvasGroup
    public TMP_Text failText;          // F_Image 안의 텍스트
    public string failMessage = "퍼즐 실패! 다시 도전하세요!";

    public float fadeDuration = 1.5f;
    public float messageDelay = 1f;

    private void Awake()
    {
        if (fadePanel != null) fadePanel.SetActive(false);  // 시작 시 꺼두기
    }

    public void FadeOutAndRestart()
    {
        Debug.Log("✅ FadeOutAndRestart 호출됨");

        if (fadePanel != null && !fadePanel.activeInHierarchy)
        {
            fadePanel.SetActive(true);  // 패널 켜주기
        }

        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;       // alpha 초기화
        }

        if (failText != null)
        {
            failText.gameObject.SetActive(false);  // 텍스트 숨기기
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
}