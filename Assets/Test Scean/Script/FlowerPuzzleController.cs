using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 🌸 FlowerPuzzleController
/// 꽃잎 낙하 퍼즐을 관리하고 퍼즐 성공 시 꿀을 생성하는 역할
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
    public ButterflyController butterfly;
    public int moveLimit = 5;
    private int currentMoveCount = 0;

    [Header("🎯 퍼즐 클리어 설정")]
    public GameObject clearImage;
    private bool isPuzzleCleared = false;

    [Header("🍯 퍼즐 성공 시 꿀 생성 설정")]
    [Tooltip("퍼즐 성공 시 꿀이 나올 특별한 꽃 오브젝트입니다.")]
    public GameObject honeyFlower; // 제일 왼쪽에 있는 꿀 꽃
    [Tooltip("생성할 꿀 이펙트 프리팹입니다.")]
    public GameObject honeyPrefab; // 꿀 이펙트 프리팹
    [Tooltip("꿀 생성 위치 오프셋입니다.")]
    public Vector3 honeySpawnOffset = new Vector3(0, 0.5f, 0);

    /// <summary>
    /// 꽃 클릭 시 호출: 나비 이동
    /// </summary>
    public void OnFlowerClicked(FlowerController clickedFlower)
    {
        if (isPuzzleCleared) return;
        if (butterfly.IsMoving()) return;

        if (currentMoveCount >= moveLimit)
        {
            Debug.Log("❌ 이동 횟수 초과! 퍼즐 실패");
            FailPuzzle();
            return;
        }

        currentMoveCount++;
        butterfly.MoveToFlower(clickedFlower);
    }

    /// <summary>
    /// 퍼즐 성공 체크
    /// </summary>
    public void CheckPuzzleStatus()
    {
        if (isPuzzleCleared) return;

        if (flower1.currentPetalCount == targetPetalCount_Flower1 &&
            flower2.currentPetalCount == targetPetalCount_Flower2 &&
            flower3.currentPetalCount == targetPetalCount_Flower3 &&
            flower4.currentPetalCount == targetPetalCount_Flower4)
        {
            Debug.Log("🎉 퍼즐 성공! 마법의 꿀 획득");

            isPuzzleCleared = true;

            if (clearImage != null)
            {
                clearImage.SetActive(true);
            }

            // 🌟 퍼즐 성공 시 꿀 생성
            SpawnHoney();
        }
    }

    /// <summary>
    /// 퍼즐 실패 시 호출
    /// </summary>
    public void FailPuzzle()
    {
        Debug.Log("❌ 퍼즐 실패! 다시 시작합니다.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 꿀 생성 함수 (퍼즐 성공 시 호출)
    /// </summary>
    private void SpawnHoney()
    {
        if (honeyFlower != null && honeyPrefab != null)
        {
            Vector3 spawnPos = honeyFlower.transform.position + honeySpawnOffset;
            Instantiate(honeyPrefab, spawnPos, Quaternion.identity);
            Debug.Log("🍯 꿀 생성 완료!");
        }
        else
        {
            Debug.LogWarning("⚠️ 꿀 꽃이나 꿀 프리팹이 연결되어 있지 않습니다!");
        }
    }
}
