using System.Collections;
using UnityEngine;

/// <summary>
/// 🍷 와인 병 하나를 제어하는 스크립트
/// 클릭 → 이동 → 기울이기 → 따르기 → 원위치 → 퍼즐 매니저로 색상 전달
/// </summary>
public class WineBottle : MonoBehaviour
{
    [Header("🎯 따르는 위치")]
    public Transform pourTargetTransform;  // 병이 이동할 쉐이커 위치 (목표 지점)

    [Header("🧩 쉐이커 관련")]
    public SpriteRenderer shakerClosedRenderer;  // 닫힌 쉐이커 이미지
    public SpriteRenderer shakerOpenRenderer;    // 열린 쉐이커 이미지
    public float shakerFadeDuration = 0.5f;      // 쉐이커 전환 페이드 속도 (초)

    [Header("🍷 와인 따르기 연출")]
    public GameObject pourEffect;        // 따르는 이펙트 (선, 파티클 등)
    public float pourTime = 1.0f;        // 따르기 연출 시간 (초)

    [Header("🕐 이동 속도 & 곡선 높이")]
    public float moveSpeed = 2.0f;       // 병 이동 속도
    public float arcHeight = 2.0f;       // 곡선 이동 최고 높이

    [Header("🍷 와인병 이미지 (직립/기울임)")]
    public SpriteRenderer wineBottleUprightRenderer;  // 직립 상태 이미지
    public SpriteRenderer wineBottleTiltedRenderer;   // 기울임 상태 이미지
    public float bottleFadeDuration = 0.5f;          // 병 전환 페이드 속도

    [Header("🍾 퍼즐용 와인 색상 이름 (예: Gold, Red, Green)")]
    public string wineColor;  // 이 병의 와인 색상 이름

    private Vector3 originalPosition;    // 병의 원래 위치
    private Quaternion originalRotation; // 병의 원래 회전값
    private bool isRunning = false;      // 현재 연출 중 여부
    private static bool isAnyPouring = false;  // 전체에서 연출 중인지 (중복 방지용)

    private void Start()
    {
        // 👉 시작할 때 원래 위치와 회전값 저장
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // 👉 따르기 이펙트 비활성화
        if (pourEffect != null)
            pourEffect.SetActive(false);

        // 👉 쉐이커: 닫힌 상태 보이게, 열린 상태 숨김
        shakerClosedRenderer.color = new Color(1, 1, 1, 1);
        shakerOpenRenderer.color = new Color(1, 1, 1, 0);

        // 👉 병: 직립 상태 보이게, 기울임 상태 숨김
        wineBottleUprightRenderer.color = new Color(1, 1, 1, 1);
        wineBottleTiltedRenderer.color = new Color(1, 1, 1, 0);
    }

    private void OnMouseDown()
    {
        // 👉 이미 실행 중이면 클릭 무시
        if (isAnyPouring || isRunning)
            return;

        // 👉 따르기 루틴 실행
        StartCoroutine(PourRoutine());
    }

    private IEnumerator PourRoutine()
    {
        isRunning = true;
        isAnyPouring = true;

        // ⭐ 1️⃣ 목표 위치로 곡선 이동
        yield return StartCoroutine(MoveAlongCurve(pourTargetTransform.position, moveSpeed, arcHeight));

        // ⭐ 2️⃣ 직립 → 기울임 전환 (페이드)
        yield return StartCoroutine(FadeSprites(wineBottleUprightRenderer, wineBottleTiltedRenderer, bottleFadeDuration));

        // ⭐ 3️⃣ 쉐이커 닫힘 → 열림 전환 (페이드)
        yield return StartCoroutine(FadeSprites(shakerClosedRenderer, shakerOpenRenderer, shakerFadeDuration));

        // ⭐ 4️⃣ 따르기 이펙트 활성화
        if (pourEffect != null)
            pourEffect.SetActive(true);

        // ⭐ 5️⃣ 따르기 시간 대기
        yield return new WaitForSeconds(pourTime);

        // ⭐ 6️⃣ 따르기 이펙트 비활성화
        if (pourEffect != null)
            pourEffect.SetActive(false);

        // ⭐ 7️⃣ 쉐이커 열림 → 닫힘 전환 (페이드)
        yield return StartCoroutine(FadeSprites(shakerOpenRenderer, shakerClosedRenderer, shakerFadeDuration));

        // ⭐ 8️⃣ 기울임 → 직립 전환 (페이드)
        yield return StartCoroutine(FadeSprites(wineBottleTiltedRenderer, wineBottleUprightRenderer, bottleFadeDuration));

        // ⭐ 9️⃣ 원래 위치로 곡선 이동
        yield return StartCoroutine(MoveAlongCurve(originalPosition, moveSpeed, arcHeight));

        // ⭐ 10️⃣ 퍼즐 매니저에 선택한 색상 전달
        var puzzleManager = FindObjectOfType<WinePuzzleManager>();
        if (puzzleManager != null)
        {
            // 퍼즐 매니저에 string(색상 이름) 전달
            puzzleManager.SelectWine(wineColor);
        }
        else
        {
            Debug.LogWarning("⚠ WinePuzzleManager가 씬에 없습니다. Inspector 연결 또는 FindObjectOfType 확인 필요.");
        }

        isRunning = false;
        isAnyPouring = false;
    }

    /// <summary>
    /// ⭐ 곡선으로 이동시키는 코루틴 (베지어 곡선)
    /// </summary>
    private IEnumerator MoveAlongCurve(Vector3 targetPosition, float speed, float height)
    {
        Vector3 startPoint = transform.position;
        Vector3 controlPoint1 = startPoint + new Vector3(0, height, 0);
        Vector3 controlPoint2 = targetPosition + new Vector3(0, height, 0);
        float t = 0f;
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
    /// ⭐ SpriteRenderer 간 페이드 전환 코루틴
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



