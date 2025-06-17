using System.Collections;
using UnityEngine;

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

    [Header("🍷 와인병 이미지")]
    public SpriteRenderer wineBottleUprightRenderer;
    public SpriteRenderer wineBottleTiltedRenderer;
    public float bottleFadeDuration = 0.5f;

    [Header("🍾 퍼즐용 와인 색상 이름")]
    public string wineColor;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isRunning = false;
    private static bool isAnyPouring = false;
    private bool isLocked = false;  // ⭐ 클릭 잠금 여부

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (pourEffect != null)
            pourEffect.SetActive(false);

        shakerClosedRenderer.color = new Color(1, 1, 1, 1);
        shakerOpenRenderer.color = new Color(1, 1, 1, 0);

        wineBottleUprightRenderer.color = new Color(1, 1, 1, 1);
        wineBottleTiltedRenderer.color = new Color(1, 1, 1, 0);
    }

    private void OnMouseDown()
    {
        if (isAnyPouring || isRunning || isLocked)
            return;

        StartCoroutine(PourRoutine());
    }

    private IEnumerator PourRoutine()
    {
        isRunning = true;
        isAnyPouring = true;

        yield return StartCoroutine(MoveAlongCurve(pourTargetTransform.position, moveSpeed, arcHeight));
        yield return StartCoroutine(FadeSprites(wineBottleUprightRenderer, wineBottleTiltedRenderer, bottleFadeDuration));
        yield return StartCoroutine(FadeSprites(shakerClosedRenderer, shakerOpenRenderer, shakerFadeDuration));

        if (pourEffect != null)
            pourEffect.SetActive(true);

        yield return new WaitForSeconds(pourTime);

        if (pourEffect != null)
            pourEffect.SetActive(false);

        yield return StartCoroutine(FadeSprites(shakerOpenRenderer, shakerClosedRenderer, shakerFadeDuration));
        yield return StartCoroutine(FadeSprites(wineBottleTiltedRenderer, wineBottleUprightRenderer, bottleFadeDuration));
        yield return StartCoroutine(MoveAlongCurve(originalPosition, moveSpeed, arcHeight));

        var puzzleManager = FindObjectOfType<WinePuzzleManager>();
        if (puzzleManager != null)
        {
            puzzleManager.SelectWine(wineColor);
        }

        isRunning = false;
        isAnyPouring = false;
    }

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

    /// <summary>
    /// 병 클릭을 막는 외부용 메서드
    /// </summary>
    public void LockBottle()
    {
        isLocked = true;
    }
}
