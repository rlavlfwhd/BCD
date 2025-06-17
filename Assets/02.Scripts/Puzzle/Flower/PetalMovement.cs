using UnityEngine;

public class PetalMovement : MonoBehaviour
{
    [Header("🍃 꽃잎 이동 속성")]
    public float fallSpeed = 1f;                // 꽃잎이 아래로 떨어지는 기본 속도 (1초당 이동 거리)
    public float horizontalAmplitude = 0.2f;    // 좌우로 흔들릴 때 최대 이동 폭 (진폭)
    public float horizontalFrequency = 2f;      // 좌우 흔들림 속도 (1초 동안 몇 번 흔들릴지)

    [Header("🍃 꽃잎 페이드 아웃 설정")]
    public float lifeTime = 3f;                 // 꽃잎이 존재할 총 시간 (초)
    public float fadeDuration = 1f;             // 꽃잎이 사라지기 시작해서 완전히 사라질 때까지 걸리는 시간 (초)

    private float elapsed = 0f;                  // 생성 이후 지난 시간
    private SpriteRenderer spriteRenderer;       // 이 오브젝트에 붙은 SpriteRenderer 컴포넌트 참조

    /// <summary>
    /// 초기화 함수 - 컴포넌트 가져오기
    /// </summary>
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // 시작할 때 SpriteRenderer 컴포넌트 가져오기
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수 - 이동, 페이드아웃, 삭제 처리
    /// </summary>
    private void Update()
    {
        elapsed += Time.deltaTime; // 프레임마다 경과 시간 누적

        // ------------------------
        // 🌿 꽃잎 이동 처리
        // ------------------------

        Vector3 pos = transform.position; // 현재 위치 가져오기

        pos.y -= fallSpeed * Time.deltaTime; // 아래로 떨어지게 y축 감소
        pos.x += Mathf.Sin(Time.time * horizontalFrequency) * horizontalAmplitude * Time.deltaTime;
        // 좌우로 부드럽게 흔들리면서 이동 (sine 함수 기반)

        transform.position = pos; // 계산된 위치 적용

        // ------------------------
        // 🌸 꽃잎 페이드 아웃 처리
        // ------------------------

        // 꽃잎이 사라지기 시작해야 할 시간이 되었으면
        if (elapsed >= lifeTime - fadeDuration)
        {
            float fadeAmount = 1f - ((elapsed - (lifeTime - fadeDuration)) / fadeDuration);
            // fadeAmount: 1 → 0으로 점점 감소 (시간에 따라)

            if (spriteRenderer != null)
            {
                // 꽃잎의 색상 알파(투명도) 조정
                spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Clamp01(fadeAmount));
            }
        }

        // ------------------------
        // 🧹 꽃잎 삭제 처리
        // ------------------------

        // 설정된 수명이 다 되면
        if (elapsed >= lifeTime)
        {
            Destroy(gameObject); // 꽃잎 오브젝트 삭제
        }
    }
}

