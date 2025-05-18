using System.Collections;
using UnityEngine;

public class WineGlassController : MonoBehaviour
{
    [Header("🍷 유리잔")]
    public SpriteRenderer glassRenderer;

    [Header("🍷 스프라이트")]
    public Sprite emptyGlassSprite;
    public Sprite filledGlassSprite;    // 무지개 와인
    public Sprite weirdWineSprite;      // 실패 시 보라색 와인

    private void Start()
    {
        if (glassRenderer != null && emptyGlassSprite != null)
        {
            glassRenderer.sprite = emptyGlassSprite;
            glassRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    /// <summary>
    /// 퍼즐 성공 시: 무지개 와인 즉시 표시
    /// </summary>
    public void StartFadeInFilledGlass()
    {
        if (glassRenderer != null && filledGlassSprite != null)
        {
            glassRenderer.sprite = filledGlassSprite;
            glassRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    /// <summary>
    /// 퍼즐 실패 시: 보라색 와인 즉시 표시
    /// </summary>
    public void ShowWeirdWine()
    {
        if (glassRenderer != null && weirdWineSprite != null)
        {
            glassRenderer.sprite = weirdWineSprite;
            glassRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    /// <summary>
    /// 잔 클릭 시 → 퍼즐이 완료되었으면 스토리 이동 시도
    /// </summary>
    private void OnMouseDown()
    {
        WinePuzzleManager manager = FindObjectOfType<WinePuzzleManager>();
        if (manager != null && (manager.IsPuzzleCompleted() || manager.IsWeirdWineCreated()))
        {
            manager.TryGoToStory();
        }
        else
        {
            Debug.Log("⚠ 퍼즐이 아직 완료되지 않아 스토리 이동 불가");
        }
    }
}
