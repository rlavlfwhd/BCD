using System.Collections;
using UnityEngine;

/// <summary>
/// 쉐이커 흔들림 및 잔으로 이동 후 따르기 연출을 담당하는 컨트롤러
/// </summary>
public class ShakeController : MonoBehaviour
{
    [Header("💫 흔들림 설정")]
    public float shakeDuration = 1f;       // 흔들리는 총 시간
    public float shakeMagnitude = 0.1f;    // X축 기준 흔들림 강도
    public float shakeSpeed = 20f;         // 흔들림 속도 (진동 주기)
    public float tiltAngle = 10f;          // 흔들림 시 Z축 회전 각도

    [Header("📈 위로 이동 설정")]
    public float riseHeight = 0.5f;        // 흔들기 전에 위로 뜨는 높이
    public float riseSpeed = 3f;           // 위로 올라가는 속도 (Lerp 계수)

    [Header("🍷 따를 잔 위치")]
    public Transform glassTarget;          // 따를 대상 잔의 위치

    [Header("🍾 따르기 연출 설정")]
    public GameObject pourEffect;          // 따르기 연출용 오브젝트 (파티클 등)
    public float pourDuration = 1f;        // 따르기 유지 시간
    public float moveDuration = 1f;        // 잔으로 이동 및 복귀 소요 시간
    public float pourTiltAngle = -30f;     // 따를 때 기울이는 각도 (Z축 음수)

    private Vector3 originalPosition;      // 쉐이커 초기 위치
    private Quaternion originalRotation;   // 쉐이커 초기 회전값

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (pourEffect != null)
            pourEffect.SetActive(false);
    }

    /// <summary>
    /// 외부에서 호출되는 쉐이커 흔들기 함수
    /// </summary>
    public void StartShaking()
    {
        StartCoroutine(ShakeRoutine());
    }

    /// <summary>
    /// 흔들기 코루틴 (위로 올라간 후 흔들림)
    /// </summary>
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

        // 흔들림 실행
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float xOffset = Mathf.Sin(elapsed * shakeSpeed) * shakeMagnitude;
            float zRotation = Mathf.Sin(elapsed * shakeSpeed) * tiltAngle;

            transform.position = targetPos + new Vector3(xOffset, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, zRotation);

            yield return null;
        }

        // 회전 원상복구
        transform.rotation = originalRotation;

        // 잔으로 이동 및 따르기 실행
        StartCoroutine(MoveAndPourRoutine());
    }

    /// <summary>
    /// 잔으로 이동 → 따르기 → 복귀 루틴
    /// </summary>
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
        transform.rotation = Quaternion.Euler(0, 0, pourTiltAngle); // 따르기용 기울이기

        // 따르기 연출 시작
        if (pourEffect != null)
            pourEffect.SetActive(true);

        yield return new WaitForSeconds(pourDuration);

        if (pourEffect != null)
            pourEffect.SetActive(false);

        transform.rotation = originalRotation; // 회전 원상복귀

        // 원래 위치로 돌아감
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




