using UnityEngine;

public class FairyController : MonoBehaviour
{
    [Header("🧚 페어리 이동 속도")]
    public float moveSpeed = 3f; // 페어리의 기본 직선 이동 속도 (1초당 몇 단위 이동할지)

    [Header("🌈 아치 궤적 설정")]
    public float arcHeight = 0.5f; // 이동 경로 중간에 솟아오를 최대 높이 (포물선 높이)

    [Header("🌬️ 위아래 흔들림 설정")]
    public float verticalWaveAmplitude = 0.2f; // 위아래 흔들림의 진폭 (얼마나 위아래로 출렁일지)
    public float verticalWaveFrequency = 2f;   // 위아래 흔들림의 주기 (1초 동안 몇 번 흔들릴지)

    // 이동 시작 위치
    private Vector3 startPos;
    // 이동 목표 위치
    private Vector3 endPos;
    // 이동 경과 시간
    private float travelTime = 0f;
    // 총 이동에 걸리는 예상 시간
    private float totalTime = 1f;
    // 현재 이동 중인지 여부
    private bool isMoving = false;

    /// <summary>
    /// 꽃 오브젝트를 목표로 이동하는 함수
    /// </summary>
    public void MoveToFlower(FlowerController flower)
    {
        if (flower.arrivalPoint != null)
            MoveToPosition(flower.arrivalPoint.position); // arrivalPoint가 있을 경우 해당 지점으로 이동
        else
            MoveToPosition(flower.transform.position);    // 없으면 꽃 자체 위치로 이동
    }

    /// <summary>
    /// 특정 위치로 이동하는 함수
    /// </summary>
    public void MoveToPosition(Vector3 destination)
    {
        startPos = transform.position;          // 현재 위치를 시작점으로 저장
        endPos = destination;                   // 이동 목표 위치 설정
        float distance = Vector3.Distance(startPos, endPos); // 시작점과 끝점 사이 거리 계산
        totalTime = distance / moveSpeed;        // 총 이동 시간 = 거리 ÷ 속도
        travelTime = 0f;                         // 경과 시간 초기화
        isMoving = true;                         // 이동 시작
    }

    /// <summary>
    /// 현재 페어리가 이동 중인지 확인하는 함수
    /// </summary>
    public bool IsMoving() => isMoving;

    /// <summary>
    /// 매 프레임 이동 로직을 업데이트하는 함수
    /// </summary>
    private void Update()
    {
        if (!isMoving) return; // 이동 중이 아니라면 아무것도 하지 않음

        travelTime += Time.deltaTime; // 경과 시간 누적
        float t = Mathf.Clamp01(travelTime / totalTime); // 이동 비율(0~1)로 환산

        // 기본 직선 이동 계산
        Vector3 linearPos = Vector3.Lerp(startPos, endPos, t);

        // 아치 궤적 추가 (포물선 형태)
        float arc = 4 * arcHeight * t * (1 - t);
        // t가 0 또는 1일 때 0, t가 0.5일 때 arcHeight 만큼 상승
        linearPos.y += arc;

        // 추가적인 위아래 흔들림 (sine 파형으로 자연스러운 흔들림)
        float wave = Mathf.Sin(Time.time * verticalWaveFrequency) * verticalWaveAmplitude;
        linearPos.y += wave;

        // 최종 위치 적용
        transform.position = linearPos;

        // 목표 지점에 도달했으면 이동 종료
        if (t >= 1f)
        {
            isMoving = false;
        }
    }
}












