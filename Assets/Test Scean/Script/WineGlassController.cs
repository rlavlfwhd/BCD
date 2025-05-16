using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WineGlassController : MonoBehaviour
{
    [Header("🍷 유리잔")]
    public SpriteRenderer glassRenderer;

    [Header("🍷 스프라이트")]
    public Sprite emptyGlassSprite;
    public Sprite filledGlassSprite;

    [Header("🕐 페이드 설정")]
    public float fadeDuration = 1f;

    [Header("📖 퍼즐 성공 시 이동할 스토리 번호")]
    public int nextStoryIndex = 0;

    [Header("🖼️ 오버레이 이미지 (페이드용)")]
    public GameObject overlayImage;

    private bool isFilled = false;

    private void Start()
    {
        if (glassRenderer != null && emptyGlassSprite != null)
        {
            glassRenderer.sprite = emptyGlassSprite;
            glassRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    public void StartFadeInFilledGlass()
    {
        StartCoroutine(FadeInFilledGlass());
    }

    private IEnumerator FadeInFilledGlass()
    {
        if (glassRenderer == null || filledGlassSprite == null)
            yield break;

        glassRenderer.sprite = filledGlassSprite;

        Color color = glassRenderer.color;
        color.a = 0f;
        glassRenderer.color = color;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            glassRenderer.color = color;
            yield return null;
        }

        glassRenderer.color = new Color(1, 1, 1, 1);
        isFilled = true; // ✅ 클릭 활성화 조건
    }

    private void OnMouseDown()
    {
        // ✅ 퍼즐이 완료되어 잔이 채워졌을 때만 반응
        if (!isFilled) return;

        var manager = FindObjectOfType<WinePuzzleManager>();
        if (manager != null && manager.IsPuzzleCompleted())
        {
            StartCoroutine(GoToStoryAfterDelay(2f));
        }
        else
        {
            Debug.Log("⚠ 퍼즐이 아직 완료되지 않았습니다.");
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        if (overlayImage != null)
        {
            overlayImage.SetActive(true);
            SpriteRenderer overlay = overlayImage.GetComponent<SpriteRenderer>();
            if (overlay != null)
            {
                Color color = overlay.color;
                color.a = 0f;
                overlay.color = color;

                float timer = 0f;
                float fadeDuration = 1f;

                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                    overlay.color = color;
                    yield return null;
                }

                color.a = 1f;
                overlay.color = color;
            }
        }

        yield return new WaitForSeconds(delay);

        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }
}
