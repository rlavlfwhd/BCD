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
    public FairyController fairy;              // 이동시킬 페어리 오브젝트
    public int moveLimit = 5;                 // 총 이동 가능 횟수
    private int currentMoveCount = 0;         // 현재까지 이동한 횟수
    private FlowerController currentTargetFlower; // 현재 페어리가 이동할 타깃 꽃

    [Header("🎉 퍼즐 클리어 시 보여줄 이미지")]
    public GameObject clearImage;             // 퍼즐 성공 시 표시할 UI
    private bool isCleared = false;           // 퍼즐을 클리어했는지 여부

    [Header("🍯 퍼즐 성공 시 꿀 생성")]
    public GameObject honeyPrefab;            // 생성할 꿀 프리팹
    public Transform honeyFlower;             // 기본 장미 위치 (페어리 이동 목적)

    [Header("🌹 장미 도착 지점 (수동 지정 가능)")]
    public Transform honeyArrivalPoint;       // 페어리가 도착할 정확한 위치

    [Header("🍯 꿀 생성 위치 (수동 지정 가능)")]
    public Transform honeySpawnPoint;         // 꿀이 생성될 위치

    [Header("📽 페이드 연출 컨트롤러")]
    public FadeController fadeController;     // 페이드 아웃 전환을 담당하는 컨트롤러

    private bool isMovingToHoney = false;     // 현재 페어리가 장미로 이동 중인지 여부

    /// <summary>
    /// 꽃을 클릭했을 때 호출되는 함수. 페어리를 해당 꽃으로 이동시킴.
    /// </summary>
    public void OnFlowerClicked(FlowerController target)
    {
        // 퍼즐이 이미 클리어 되었거나 페어리가 이동 중이라면 클릭 무시
        if (isCleared || fairy.IsMoving()) return;

        // 이동 횟수를 초과한 경우 퍼즐 실패 처리
        if (currentMoveCount >= moveLimit)
        {
            Debug.Log("❌ 이동 횟수 초과. 퍼즐 실패!");
            FailPuzzle();
            return;
        }

        // 이동 횟수 증가 및 대상 꽃 설정 후 이동 명령
        currentMoveCount++;
        currentTargetFlower = target;
        fairy.MoveToFlower(target);
    }

    /// <summary>
    /// 모든 꽃이 목표 꽃잎 수와 일치하는지 검사. 클리어 여부 판정.
    /// </summary>
    public void CheckPuzzleStatus()
    {
        if (isCleared) return; // 이미 클리어된 경우 무시

        // 각 꽃의 현재 꽃잎 수가 정답과 일치하는지 확인
        if (flower1.currentPetalCount == targetPetalCount1 &&
            flower2.currentPetalCount == targetPetalCount2 &&
            flower3.currentPetalCount == targetPetalCount3 &&
            flower4.currentPetalCount == targetPetalCount4)
        {
            Debug.Log("🎉 퍼즐 클리어!");
            isCleared = true; // 상태 변경

            if (clearImage != null)
                clearImage.SetActive(true); // 클리어 UI 표시

            MoveFairyToHoney(); // 페어리를 장미 쪽으로 이동시킴
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
    /// 퍼즐 실패 시 페이드 아웃 후 씬 재시작. 페이드 연출이 없으면 즉시 재시작.
    /// </summary>
    public void FailPuzzle()
    {
        if (fadeController != null)
        {
            Debug.Log("🕶️ 페이드 아웃 후 재시작 중...");
            fadeController.FadeOutAndRestart(); // 페이드 후 씬 로드
        }
        else
        {
            Debug.LogWarning("⚠️ FadeController 미설정. 즉시 씬 재시작.");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /// <summary>
    /// 퍼즐 클리어 시 페어리를 장미 도착 지점으로 이동시킴.
    /// </summary>
    private void MoveFairyToHoney()
    {
        if (fairy == null) return;

        isMovingToHoney = true;

        // 도착 지점이 수동으로 설정되어 있으면 거기로 이동
        if (honeyArrivalPoint != null)
        {
            fairy.MoveToPosition(honeyArrivalPoint.position);
        }
        // 아니면 기본 장미 위치로 이동
        else if (honeyFlower != null)
        {
            fairy.MoveToPosition(honeyFlower.position);
        }

        Debug.Log("🧚 페어리가 장미로 이동 시작!");
    }

    /// <summary>
    /// 매 프레임마다 상태 체크하여 이동 완료 후 작업 수행
    /// </summary>
    private void Update()
    {
        // 장미 도착 완료 → 꿀 생성
        if (isMovingToHoney && !fairy.IsMoving())
        {
            SpawnHoney(); // 꿀 생성
            isMovingToHoney = false;
            return;
        }

        // 일반 꽃 도착 완료 → 꽃잎 떨어뜨리기
        if (!isCleared && currentTargetFlower != null && !fairy.IsMoving())
        {
            currentTargetFlower.DropPetal(); // 꽃잎 하나 감소
            CheckPuzzleStatus();             // 퍼즐 성공 여부 검사
            currentTargetFlower = null;      // 다음 클릭을 위해 초기화
        }
    }
}





