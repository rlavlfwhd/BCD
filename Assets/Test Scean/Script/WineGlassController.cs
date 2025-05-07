using UnityEngine;

/// <summary>
/// 와인잔 상태 관리 + 디버그 로그 출력용 컨트롤러
/// </summary>
public class WineGlassController : MonoBehaviour
{
    [Header("🍷 유리잔 본체")]
    public SpriteRenderer glassRenderer; // 유리잔 테두리 SpriteRenderer

    [Header("🍷 내용물 (빈 상태/채워진 상태)")]
    public SpriteRenderer emptyGlassRenderer; // 빈 잔 SpriteRenderer
    public SpriteRenderer filledGlassRenderer; // 채워진 잔 SpriteRenderer

    [Header("🍷 채우기 연출 속도")]
    public float fadeDuration = 1f; // 페이드 인/아웃 속도

    private void Start()
    {
        Debug.Log("==== WineGlassController 디버그 시작 ====");

        // 1️⃣ SpriteRenderer 활성 상태 체크
        Debug.Log($"[빈 잔] enabled: {emptyGlassRenderer.enabled}");
        Debug.Log($"[채운 잔] enabled: {filledGlassRenderer.enabled}");

        // 2️⃣ Sprite 연결 상태 체크
        Debug.Log($"[빈 잔] Sprite 연결됨: {(emptyGlassRenderer.sprite != null)}");
        Debug.Log($"[채운 잔] Sprite 연결됨: {(filledGlassRenderer.sprite != null)}");

        // 3️⃣ Sorting Layer & Order 체크
        Debug.Log($"[빈 잔] Sorting Layer: {emptyGlassRenderer.sortingLayerName}, Order: {emptyGlassRenderer.sortingOrder}");
        Debug.Log($"[채운 잔] Sorting Layer: {filledGlassRenderer.sortingLayerName}, Order: {filledGlassRenderer.sortingOrder}");

        Debug.Log("========================================");

        // 빈 잔만 켜고 채운 잔은 처음에 꺼놓기
        emptyGlassRenderer.enabled = true;
        filledGlassRenderer.enabled = true; // 반드시 켜야 알파로 페이드가 작동함
        SetAlpha(emptyGlassRenderer, 1f);
        SetAlpha(filledGlassRenderer, 0f);
    }

    /// <summary>
    /// 외부에서 호출: 잔 채우기 페이드 인
    /// </summary>
    public void FadeInFilledGlass()
    {
        StartCoroutine(FadeSprites(emptyGlassRenderer, filledGlassRenderer, fadeDuration));
    }

    /// <summary>
    /// 페이드 아웃 & 인 코루틴
    /// </summary>
    private System.Collections.IEnumerator FadeSprites(SpriteRenderer fromRenderer, SpriteRenderer toRenderer, float duration)
    {
        float elapsed = 0f;

        // ✅ 반드시 SpriteRenderer.enabled가 true여야 페이드가 보임
        fromRenderer.enabled = true;
        toRenderer.enabled = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            SetAlpha(fromRenderer, Mathf.Lerp(1, 0, t));
            SetAlpha(toRenderer, Mathf.Lerp(0, 1, t));

            Debug.Log($"페이드 진행 중... t={t:F2}, fromAlpha={Mathf.Lerp(1, 0, t):F2}, toAlpha={Mathf.Lerp(0, 1, t):F2}");

            yield return null;
        }

        SetAlpha(fromRenderer, 0f);
        SetAlpha(toRenderer, 1f);

        Debug.Log("✅ 페이드 완료!");
    }

    /// <summary>
    /// SpriteRenderer 알파값 설정
    /// </summary>
    private void SetAlpha(SpriteRenderer renderer, float alpha)
    {
        if (renderer != null)
        {
            Color c = renderer.color;
            c.a = alpha;
            renderer.color = c;
        }
    }
}




