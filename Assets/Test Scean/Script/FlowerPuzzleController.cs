using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerPuzzleController : MonoBehaviour
{
    [Header("🌸 퍼즐에 사용될 4개의 꽃 오브젝트")]
    public FlowerController flower1;
    public FlowerController flower2;
    public FlowerController flower3;
    public FlowerController flower4;

    [Header("🎯 각 꽃이 맞춰야 할 정답 꽃잎 개수")]
    public int targetPetalCount1 = 3;
    public int targetPetalCount2 = 4;
    public int targetPetalCount3 = 2;
    public int targetPetalCount4 = 5;

    [Header("🧚 페어리 이동 및 횟수 제한")]
    public FairyController fairy;
    public int moveLimit = 5;
    private int currentMoveCount = 0;
    private FlowerController currentTargetFlower;

    [Header("🎉 퍼즐 클리어 시 보여줄 이미지")]
    public GameObject clearImage;
    private bool isCleared = false;

    [Header("🍯 퍼즐 성공 시 꿀 생성")]
    public GameObject honeyPrefab;
    public Transform honeyFlower;

    [Header("🌹 장미 도착 지점 (수동 지정 가능)")]
    public Transform honeyArrivalPoint;

    [Header("🍯 꿀 생성 위치 (수동 지정 가능)")]
    public Transform honeySpawnPoint;

    [Header("📽 페이드 연출 컨트롤러")]
    public FadeController fadeController;

    private bool isMovingToHoney = false;

    /// <summary>
    /// 꽃을 클릭했을 때 호출되는 함수. 페어리를 해당 꽃으로 이동시킴.
    /// </summary>
    public void OnFlowerClicked(FlowerController target)
    {
        if (isCleared || fairy.IsMoving()) return;

        if (currentMoveCount >= moveLimit)
        {
            Debug.Log("❌ 이동 횟수 초과. 퍼즐 실패!");
            FailPuzzle();
            return;
        }

        currentMoveCount++;
        currentTargetFlower = target;
        fairy.MoveToFlower(target);
    }

    /// <summary>
    /// 모든 꽃이 목표 꽃잎 수와 일치하는지 검사. 클리어 여부 판정.
    /// </summary>
    public void CheckPuzzleStatus()
    {
        if (isCleared) return;

        if (flower1.currentPetalCount == targetPetalCount1 &&
            flower2.currentPetalCount == targetPetalCount2 &&
            flower3.currentPetalCount == targetPetalCount3 &&
            flower4.currentPetalCount == targetPetalCount4)
        {
            Debug.Log("🎉 퍼즐 클리어!");
            isCleared = true;

            if (clearImage != null)
                clearImage.SetActive(true);

            MoveFairyToHoney();
        }
    }

    /// <summary>
    /// 꿀 프리팹을 꿀 생성 지점에 생성
    /// </summary>
    public void SpawnHoney()
    {
        if (honeyPrefab != null && honeySpawnPoint != null)
        {
            Instantiate(honeyPrefab, honeySpawnPoint.position, Quaternion.identity);
            Debug.Log("🍯 꿀 생성 완료! 위치: " + honeySpawnPoint.position);
        }
        else
        {
            Debug.LogWarning("⚠️ 꿀 프리팹 또는 생성 위치가 설정되지 않았습니다.");
        }
    }

    /// <summary>
    /// 퍼즐 실패 시 페이드 아웃 후 씬 재시작
    /// </summary>
    public void FailPuzzle()
    {
        if (fadeController != null)
        {
            Debug.Log("🕶️ 페이드 아웃 후 재시작 중...");

            GameObject fadeObj = fadeController.gameObject;
            if (!fadeObj.activeInHierarchy)
                fadeObj.SetActive(true); // 반드시 먼저 활성화

            fadeController.FadeOutAndRestart(); // 페이드 실행
        }
        else
        {
            Debug.LogWarning("⚠️ FadeController 미설정. 즉시 씬 재시작.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /// <summary>
    /// 퍼즐 클리어 시 페어리를 꿀 위치로 이동시킴
    /// </summary>
    private void MoveFairyToHoney()
    {
        if (fairy == null) return;

        isMovingToHoney = true;

        if (honeyArrivalPoint != null)
            fairy.MoveToPosition(honeyArrivalPoint.position);
        else if (honeyFlower != null)
            fairy.MoveToPosition(honeyFlower.position);

        Debug.Log("🧚 페어리가 장미로 이동 시작!");
    }

    /// <summary>
    /// 매 프레임 상태 확인하여 작업 실행
    /// </summary>
    private void Update()
    {
        // 클리어 후 장미 도착 시 꿀 생성
        if (isMovingToHoney && !fairy.IsMoving())
        {
            SpawnHoney();
            isMovingToHoney = false;
            return;
        }

        // 일반 꽃 클릭 → 이동 완료 후 꽃잎 감소 및 체크
        if (!isCleared && currentTargetFlower != null && !fairy.IsMoving())
        {
            currentTargetFlower.DropPetal();
            CheckPuzzleStatus();
            currentTargetFlower = null;
        }
    }
}




