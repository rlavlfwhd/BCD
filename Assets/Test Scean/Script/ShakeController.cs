using System.Collections;
using UnityEngine;

/// <summary>
/// 쉐이커 흔들림 및 잔으로 이동 후 따르기 연출을 담당하는 컨트롤러
/// </summary>
public class ShakeController : MonoBehaviour
{
    [Header("💫 흔들림 설정")]
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.1f;
    public float shakeSpeed = 20f;
    public float tiltAngle = 10f;

    [Header("📈 위로 이동 설정")]
    public float riseHeight = 0.5f;
    public float riseSpeed = 3f;

    [Header("🍷 따를 잔 위치")]
    public Transform glassTarget;

    [Header("🍾 따르기 연출 설정")]
    public GameObject pourEffect;
    public float pourDuration = 1f;
    public float moveDuration = 1f;
    public float pourTiltAngle = -30f;

    [Header("🍷 잔 컨트롤러 (잔 채우기 페이드 인용)")]
    public WineGlassController wineGlassController;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (pourEffect != null)
            pourEffect.SetActive(false);
    }

    /// <summary>
    /// 외부에서 호출되는 쉐이커 흔들기 (코루틴)
    /// </summary>
    public IEnumerator StartShaking()
    {
        yield return StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;
        Vector3 targetPos = originalPosition + Vector3.up * riseHeight;

        // 위로 떠오름
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * riseSpeed);
            yield return null;
        }

        // 흔들기
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float xOffset = Mathf.Sin(elapsed * shakeSpeed) * shakeMagnitude;
            float zRotation = Mathf.Sin(elapsed * shakeSpeed) * tiltAngle;

            transform.position = targetPos + new Vector3(xOffset, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, zRotation);

            yield return null;
        }

        transform.rotation = originalRotation;

        // 따르기 시작
        yield return StartCoroutine(MoveAndPourRoutine());
    }

    private IEnumerator MoveAndPourRoutine()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = glassTarget.position;
        float elapsed = 0f;

        // 잔 위치로 이동
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = Quaternion.Euler(0, 0, pourTiltAngle);

        // ⭐ 따르기 연출 + 잔 채우기 페이드 인 동기화
        if (pourEffect != null)
            pourEffect.SetActive(true);

        if (wineGlassController != null)
            wineGlassController.StartFadeInFilledGlass();

        yield return new WaitForSeconds(pourDuration);

        if (pourEffect != null)
            pourEffect.SetActive(false);

        transform.rotation = originalRotation;

        // 원래 위치로 복귀
        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(targetPos, originalPosition, elapsed / moveDuration);
            yield return null;
        }

        transform.position = originalPosition;
    }
}