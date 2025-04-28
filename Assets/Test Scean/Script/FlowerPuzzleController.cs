using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 🌸 꽃 퍼즐 전용 매니저
/// 꽃잎 개수, 이동횟수, 퍼즐 성공/실패를 관리한다
/// </summary>
public class FlowerPuzzleController : MonoBehaviour
{
    [Header("🌸 퍼즐 목표 꽃잎 수")]
    public int targetPetalCount_Flower1 = 2;
    public int targetPetalCount_Flower2 = 3;
    public int targetPetalCount_Flower3 = 1;
    public int targetPetalCount_Flower4 = 5;

    [Header("🌼 연결된 꽃 오브젝트들")]
    public FlowerController flower1;
    public FlowerController flower2;
    public FlowerController flower3;
    public FlowerController flower4;

    [Header("🦋 나비 이동 설정")]
    public ButterflyController butterfly;   // 나비 컨트롤러 연결
    public int moveLimit = 5;                // 최대 이동 가능 횟수

    private int currentMoveCount = 0;         // 현재 이동한 횟수

    [Header("🎯 퍼즐 클리어 ID")]
    public string puzzleID = "FlowerPuzzle1";

    [Header("🎉 퍼즐 성공 시 보여줄 클리어 이미지")]
    public GameObject clearImage;             // 퍼즐 성공 시 보여줄 이미지 연결

    private bool isPuzzleCleared = false;      // 퍼즐이 클리어됐는지 여부

    /// <summary>
    /// 꽃 클릭 시 호출: 이동 가능 횟수 체크 후 나비 이동
    /// </summary>
    public void OnFlowerClicked(FlowerController clickedFlower)
    {
        if (isPuzzleCleared) return; // 퍼즐 이미 클리어됐으면 무시

        // 🔥 추가: 나비가 이동 중이면 클릭 무시
        if (butterfly.IsMoving())
        {
            Debug.Log("🦋 나비가 이동 중이라 클릭 무시");
            return;
        }

        if (currentMoveCount >= moveLimit)
        {
            Debug.Log("❌ 이동 횟수 초과! 퍼즐 실패");
            FailPuzzle();
            return;
        }

        currentMoveCount++;
        Debug.Log($"🦋 나비 이동 시작! 현재 이동 횟수: {currentMoveCount}/{moveLimit}");
        butterfly.MoveToFlower(clickedFlower);
    }

    /// <summary>
    /// 퍼즐 현재 상태를 검사
    /// (꽃잎 목표 개수 맞으면 퍼즐 성공)
    /// </summary>
    public void CheckPuzzleStatus()
    {
        if (isPuzzleCleared) return; // 이미 클리어됐으면 무시

        if (flower1.currentPetalCount == targetPetalCount_Flower1 &&
            flower2.currentPetalCount == targetPetalCount_Flower2 &&
            flower3.currentPetalCount == targetPetalCount_Flower3 &&
            flower4.currentPetalCount == targetPetalCount_Flower4)
        {
            Debug.Log("🎉 퍼즐 성공! 마법의 꿀 획득");

            isPuzzleCleared = true; // 퍼즐 클리어 체크

            // 클리어 이미지 활성화
            if (clearImage != null)
            {
                clearImage.SetActive(true);
            }
        }
    }

    /// <summary>
    /// 퍼즐 실패 시 호출
    /// </summary>
    public void FailPuzzle()
    {
        Debug.Log("❌ 퍼즐 실패! 다시 시작합니다.");
        ResetPuzzle();
    }

    /// <summary>
    /// 퍼즐을 리셋 (초기 상태로 복구)
    /// </summary>
    public void ResetPuzzle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
