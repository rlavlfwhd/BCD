using UnityEngine;

/// <summary>
/// 나비를 이동시키고 꽃에 도착하면 꽃잎을 떨어뜨리는 스크립트
/// (이제 FlowerPuzzleController와 직접 연결)
/// </summary>
public class ButterflyController : MonoBehaviour
{
    [Header("🦋 나비 이동 설정")]
    public float moveSpeed = 5f;
    public int moveCount = 0;        // 현재 이동한 횟수
    public int moveLimit = 5;         // 최대 이동 가능 횟수

    private bool isMoving = false;
    private FlowerController targetFlower;
    private FlowerPuzzleController flowerPuzzleController; // 추가!

    void Start()
    {
        flowerPuzzleController = FindObjectOfType<FlowerPuzzleController>(); // 퍼즐 매니저 연결
    }

    void Update()
    {
        if (isMoving && targetFlower != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetFlower.transform.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetFlower.transform.position) < 0.1f)
            {
                isMoving = false;
                ArriveAtFlower();
            }
        }
    }

    /// <summary>
    /// 꽃 클릭 시 호출
    /// </summary>
    public void MoveToFlower(FlowerController flower)
    {
        if (moveCount >= moveLimit)
        {
            Debug.Log("❌ 이동 횟수 초과! 퍼즐 실패");
            flowerPuzzleController.FailPuzzle(); // ✅ PuzzleManager가 아니라 FlowerPuzzleController를 호출
            return;
        }

        targetFlower = flower;
        isMoving = true;
        moveCount++;
        Debug.Log($"🦋 나비 이동 시작! 현재 이동 횟수: {moveCount}/{moveLimit}");
    }

    /// <summary>
    /// 꽃에 도착하면 꽃잎을 떨어뜨리고 퍼즐 상태 체크
    /// </summary>
    private void ArriveAtFlower()
    {
        Debug.Log($"🦋 나비 {targetFlower.name}에 도착!");
        targetFlower.DropPetal();
        flowerPuzzleController.CheckPuzzleStatus(); // ✅ PuzzleManager가 아니라 FlowerPuzzleController를 호출
    }
}


