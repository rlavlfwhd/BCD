using System.Collections;
using UnityEngine;

/// <summary>
/// 🍷 와인병 이동 → 기울임 → 따르기 → 복귀
/// 직립/기울임 상태 전환 페이드 연출
/// 쉐이커 열고 닫기도 페이드
/// </summary>
public class WineBottle : MonoBehaviour
{
    [Header("🎯 따르는 위치")]
    public Transform pourTargetTransform;

    [Header("🧩 쉐이커 관련")]
    public SpriteRenderer shakerClosedRenderer;
    public SpriteRenderer shakerOpenRenderer;
    public float shakerFadeDuration = 0.5f;

    [Header("🍷 와인 따르기 연출")]
    public GameObject pourEffect;
    public float pourTime = 1.0f;

    [Header("🕐 이동 속도 & 곡선 높이")]
    public float moveSpeed = 2.0f;
    public float arcHeight = 2.0f;

    [Header("🍷 와인병 이미지 (직립/기울임)")]
    public SpriteRenderer wineBottleUprightRenderer;  // 직립 이미지
    public SpriteRenderer wineBottleTiltedRenderer;   // 기울임 이미지
    public float bottleFadeDuration = 0.5f;          // 와인병 전환 시간

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isRunning = false;
    private static bool isAnyPouring = false;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (pourEffect != null) pourEffect.SetActive(false);

        // 쉐이커 초기 상태: 닫힘 100%, 열림 0%
        shakerClosedRenderer.color = new Color(1, 1, 1, 1);
        shakerOpenRenderer.color = new Color(1, 1, 1, 0);

        // 와인병 초기 상태: 직립 100%, 기울임 0%
        wineBottleUprightRenderer.color = new Color(1, 1, 1, 1);
        wineBottleTiltedRenderer.color = new Color(1, 1, 1, 0);
    }

    private void OnMouseDown()
    {
        if (isAnyPouring || isRunning) return;
        StartCoroutine(PourRoutine());
    }

    private IEnumerator PourRoutine()
    {
        isRunning = true;
        isAnyPouring = true;

        // 1️⃣ 와인병 → 쉐이커 위 도착
        yield return StartCoroutine(MoveAlongCurve(pourTargetTransform.position, moveSpeed, arcHeight));

        // 2️⃣ 직립 → 기울임 전환 페이드
        yield return StartCoroutine(FadeSprites(wineBottleUprightRenderer, wineBottleTiltedRenderer, bottleFadeDuration));

        // 3️⃣ 쉐이커 열기
        yield return StartCoroutine(FadeSprites(shakerClosedRenderer, shakerOpenRenderer, shakerFadeDuration));

        // 4️⃣ 따르기 이펙트 켜기
        if (pourEffect != null) pourEffect.SetActive(true);

        // 5️⃣ 따르기 유지
        yield return new WaitForSeconds(pourTime);

        // 6️⃣ 따르기 이펙트 끄기
        if (pourEffect != null) pourEffect.SetActive(false);

        // 7️⃣ 쉐이커 닫기
        yield return StartCoroutine(FadeSprites(shakerOpenRenderer, shakerClosedRenderer, shakerFadeDuration));

        // 8️⃣ 기울임 → 직립 전환 페이드
        yield return StartCoroutine(FadeSprites(wineBottleTiltedRenderer, wineBottleUprightRenderer, bottleFadeDuration));

        // 9️⃣ 원래 자리로 이동
        yield return StartCoroutine(MoveAlongCurve(originalPosition, moveSpeed, arcHeight));

        isRunning = false;
        isAnyPouring = false;
    }

    /// <summary>
    /// 🍷 3차 베지어 곡선 이동
    /// </summary>
    private IEnumerator MoveAlongCurve(Vector3 targetPosition, float speed, float height)
    {
        Vector3 startPoint = transform.position;
        Vector3 controlPoint1 = startPoint + new Vector3(0, height, 0);
        Vector3 controlPoint2 = targetPosition + new Vector3(0, height, 0);

        float t = 0;
        float fixedZ = transform.position.z;

        while (t < 1.0f)
        {
            t += speed * Time.deltaTime;
            Vector3 bezierPos = Mathf.Pow(1 - t, 3) * startPoint
                              + 3 * Mathf.Pow(1 - t, 2) * t * controlPoint1
                              + 3 * (1 - t) * Mathf.Pow(t, 2) * controlPoint2
                              + Mathf.Pow(t, 3) * targetPosition;

            transform.position = new Vector3(bezierPos.x, bezierPos.y, fixedZ);
            yield return null;
        }

        transform.position = new Vector3(targetPosition.x, targetPosition.y, fixedZ);
    }

    /// <summary>
    /// ✨ 이미지 전환 페이드 (from → to)
    /// </summary>
    private IEnumerator FadeSprites(SpriteRenderer fromRenderer, SpriteRenderer toRenderer, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            fromRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, t));
            toRenderer.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        fromRenderer.color = new Color(1, 1, 1, 0);
        toRenderer.color = new Color(1, 1, 1, 1);
    }
}


