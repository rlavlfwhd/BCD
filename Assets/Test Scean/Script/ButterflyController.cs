using UnityEngine;

/// <summary>
/// 🦋 고급 Perlin Noise 기반 자연스러운 나비 이동
/// </summary>
public class ButterflyController : MonoBehaviour
{
    [Header("🦋 나비 이동 설정")]
    [Tooltip("나비가 꽃으로 이동하는 기본 속도입니다.")]
    public float moveSpeed = 3.5f;

    [Tooltip("나비가 이동할 수 있는 최대 횟수입니다.")]
    public int moveLimit = 5;

    [Header("🌀 나비 흔들림 설정")]
    [Tooltip("나비 이동 시 흔들림의 폭입니다. (값이 클수록 크게 흔들립니다)")]
    public float waveAmplitude = 0.25f;

    [Tooltip("나비 이동 시 흔들림의 부드러움 조절입니다. (값이 클수록 빠르게 변화)")]
    public float noiseSpeed = 1.5f;

    [Tooltip("나비 흔들림 방향을 설정합니다. (true: Y축, false: X축)")]
    public bool useYAxisWave = true; // true면 위아래, false면 좌우

    private bool isMoving = false;
    private FlowerController targetFlower;
    private FlowerPuzzleController flowerPuzzleController;
    private Vector3 startMovePosition;
    private Vector3 targetFlowerAdjustedPosition;
    private float moveStartTime;
    private int moveCount = 0;

    private float noiseSeed; // Perlin Noise 랜덤 시드

    void Start()
    {
        flowerPuzzleController = FindObjectOfType<FlowerPuzzleController>();
    }

    void Update()
    {
        if (isMoving && targetFlower != null)
        {
            float journeyLength = Vector3.Distance(startMovePosition, targetFlowerAdjustedPosition);
            float distCovered = (Time.time - moveStartTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;

            Vector3 straightLinePos = Vector3.Lerp(startMovePosition, targetFlowerAdjustedPosition, fractionOfJourney);

            // 🌟 Perlin Noise 기반 자연스러운 흔들림 추가
            Vector3 offset = Vector3.zero;
            float noiseValue = (Mathf.PerlinNoise(Time.time * noiseSpeed + noiseSeed, 0f) - 0.5f) * 2f;

            if (useYAxisWave)
                offset.y = noiseValue * waveAmplitude;
            else
                offset.x = noiseValue * waveAmplitude;

            transform.position = straightLinePos + offset;

            if (fractionOfJourney >= 1f)
            {
                isMoving = false;
                ArriveAtFlower();
            }
        }
    }

    public void MoveToFlower(FlowerController flower)
    {
        if (moveCount >= moveLimit)
        {
            Debug.Log("❌ 이동 횟수 초과! 퍼즐 실패");
            flowerPuzzleController.FailPuzzle();
            return;
        }

        targetFlower = flower;
        isMoving = true;
        moveCount++;

        startMovePosition = transform.position;
        Vector3 targetPos = flower.transform.position;
        targetPos.z += 0.2f;
        targetFlowerAdjustedPosition = targetPos;
        moveStartTime = Time.time;

        // 🌟 이동 시작할 때마다 noiseSeed 랜덤 설정
        noiseSeed = Random.Range(0f, 100f);

        Debug.Log($"🦋 나비 이동 시작! 현재 이동 횟수: {moveCount}/{moveLimit}");
    }

    private void ArriveAtFlower()
    {
        Debug.Log($"🦋 나비 {targetFlower.name} 도착!");
        targetFlower.DropPetal();
        flowerPuzzleController.CheckPuzzleStatus();
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}






