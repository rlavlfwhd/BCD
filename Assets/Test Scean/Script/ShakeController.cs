using System.Collections;
using UnityEngine;

public class ShakeController : MonoBehaviour
{
    [Header("🕰 흔들림 지속 시간 (초)")]
    [Tooltip("쉐이커가 흔들리는 전체 시간 (초)")]
    public float shakeDuration = 1f;

    [Header("↔ 흔들림 강도 (X축 좌우, Y축 상하)")]
    [Tooltip("쉐이커 좌우로 흔들리는 강도")]
    public float shakeMagnitudeX = 0.1f;

    [Tooltip("쉐이커 위아래로 흔들리는 강도")]
    public float shakeMagnitudeY = 0.05f;

    [Header("🏃‍♂️ 흔들림 속도")]
    [Tooltip("쉐이커가 얼마나 빠르게 흔들릴지 (진동 속도)")]
    public float shakeSpeed = 20f;

    [Header("🌀 기울기 각도 (z축 회전)")]
    [Tooltip("쉐이커가 흔들릴 때 z축으로 기울어지는 각도")]
    public float tiltAngle = 10f;

    [Header("⬆ 흔들림 전 위로 올리는 높이")]
    [Tooltip("흔들리기 전에 위로 들어올리는 높이")]
    public float liftHeight = 0.5f;

    [Tooltip("들어올릴 때 이동 속도 (초당 거리)")]
    public float liftSpeed = 2f;

    private Vector3 originalPosition;      // 시작 위치
    private Quaternion originalRotation;   // 시작 회전값

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    /// <summary>
    /// 외부에서 흔들림을 시작할 때 호출
    /// </summary>
    public void StartShaking()
    {
        StartCoroutine(ShakeRoutine());
    }

    /// <summary>
    /// 쉐이크 연출 루틴
    /// </summary>
    private IEnumerator ShakeRoutine()
    {
        Vector3 liftedPosition = originalPosition + Vector3.up * liftHeight; // 위로 올린 목표 위치

        // 1️⃣ 위로 올리기 (부드럽게)
        while (Vector3.Distance(transform.localPosition, liftedPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, liftedPosition, liftSpeed * Time.deltaTime);
            yield return null;
        }

        float elapsed = 0f; // 경과 시간

        // 2️⃣ 흔들림 실행
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            // 좌우 흔들림 계산
            float offsetX = Mathf.Sin(elapsed * shakeSpeed) * shakeMagnitudeX;

            // 상하 흔들림 계산
            float offsetY = Mathf.Sin(elapsed * shakeSpeed * 1.5f) * shakeMagnitudeY;

            // Z축 기울기 계산
            float rotationZ = Mathf.Sin(elapsed * shakeSpeed) * tiltAngle;

            // 흔들린 위치 + liftedPosition 기준
            transform.localPosition = liftedPosition + new Vector3(offsetX, offsetY, 0f);

            // 회전 적용
            transform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, rotationZ);

            yield return null;
        }

        // 3️⃣ 원래 위치로 되돌리기 (부드럽게)
        while (Vector3.Distance(transform.localPosition, originalPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, originalPosition, liftSpeed * Time.deltaTime);
            yield return null;
        }

        // 최종 위치·회전 복원
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}


