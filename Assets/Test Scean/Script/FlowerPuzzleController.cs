using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerPuzzleController : MonoBehaviour
{
    [Header("🌸 꽃 4개")]
    public FlowerController flower1; // 첫 번째 꽃
    public FlowerController flower2; // 두 번째 꽃
    public FlowerController flower3; // 세 번째 꽃
    public FlowerController flower4; // 네 번째 꽃

    [Header("🎯 목표 꽃잎 수")]
    public int targetPetalCount1 = 3; // 첫 번째 꽃의 목표 남은 꽃잎 수
    public int targetPetalCount2 = 4; // 두 번째 꽃 목표
    public int targetPetalCount3 = 2; // 세 번째 꽃 목표
    public int targetPetalCount4 = 5; // 네 번째 꽃 목표

    [Header("🧚 페어리")]
    public FairyController fairy;        // 페어리 오브젝트 (FairyController 연결)
    public int moveLimit = 5;             // 총 이동 가능 횟수
    private int currentMoveCount = 0;     // 현재 이동한 횟수
    private FlowerController currentTargetFlower; // 현재 이동하려는 꽃

    [Header("🎉 퍼즐 클리어 시 이미지")]
    public GameObject clearImage;         // 퍼즐 클리어 시 띄울 UI 이미지
    private bool isCleared = false;        // 퍼즐을 클리어했는지 여부

    [Header("🍯 퍼즐 성공 시 꿀 생성")]
    public GameObject honeyPrefab;         // 생성할 꿀 프리팹
    public Transform honeyFlower;          // 장미 Transform (기본 위치용)

    [Header("🌹 장미 도착 지점 (수동 지정)")]
    public Transform honeyArrivalPoint;    // 페어리가 장미로 이동할 정확한 위치 (빈 오브젝트로 수동 설정)

    [Header("🍯 꿀 생성 위치 (수동 지정)")]
    public Transform honeySpawnPoint;      // 꿀이 생성될 위치 (빈 오브젝트로 수동 설정)

    private bool isMovingToHoney = false;   // 현재 페어리가 장미로 이동 중인지 여부

    /// <summary>
    /// 사용자가 꽃을 클릭했을 때 호출되는 함수
    /// 페어리를 해당 꽃으로 이동시키고 이동 횟수 관리
    /// </summary>
    public void OnFlowerClicked(FlowerController target)
    {
        if (isCleared || fairy.IsMoving()) return; // 퍼즐이 끝났거나 이동 중이면 무시

        if (currentMoveCount >= moveLimit)
        {
            Debug.Log("❌ 이동 횟수 초과. 퍼즐 실패!");
            FailPuzzle(); // 이동 제한을 초과했으면 실패 처리
            return;
        }

        currentMoveCount++;             // 이동 횟수 증가
        currentTargetFlower = target;   // 이동 대상 꽃 저장
        fairy.MoveToFlower(target);     // 페어리 이동 시작
    }

    /// <summary>
    /// 현재 꽃들의 꽃잎 수를 체크하여 퍼즐 클리어 여부 확인
    /// </summary>
    public void CheckPuzzleStatus()
    {
        if (isCleared) return; // 이미 클리어했으면 무시

        // 네 개의 꽃이 모두 목표 꽃잎 수와 일치하는지 확인
        if (flower1.currentPetalCount == targetPetalCount1 &&
            flower2.currentPetalCount == targetPetalCount2 &&
            flower3.currentPetalCount == targetPetalCount3 &&
            flower4.currentPetalCount == targetPetalCount4)
        {
            Debug.Log("🎉 퍼즐 클리어!");
            isCleared = true; // 클리어 상태로 변경

            if (clearImage != null)
                clearImage.SetActive(true); // 퍼즐 클리어 이미지를 보여줌

            MoveFairyToHoney(); // 클리어했으면 장미로 이동
        }
    }

    /// <summary>
    /// 꿀을 생성하는 함수
    /// </summary>
    public void SpawnHoney()
    {
        if (honeyPrefab != null && honeySpawnPoint != null)
        {
            Instantiate(honeyPrefab, honeySpawnPoint.position, Quaternion.identity); // 꿀을 생성
            Debug.Log("🍯 꿀 생성 완료! 위치: " + honeySpawnPoint.position);
        }
        else
        {
            Debug.LogWarning("⚠️ 꿀 프리팹 또는 꿀 생성 지점(honeySpawnPoint)이 비어 있습니다."); // 설정 누락 경고
        }
    }

    /// <summary>
    /// 페어리를 장미 도착 지점으로 이동시키는 함수
    /// </summary>
    private void MoveFairyToHoney()
    {
        if (fairy == null) return; // 페어리가 없으면 아무것도 하지 않음

        isMovingToHoney = true; // 장미 이동 플래그 설정

        if (honeyArrivalPoint != null)
        {
            fairy.MoveToPosition(honeyArrivalPoint.position); // 지정된 도착 지점으로 이동
        }
        else if (honeyFlower != null)
        {
            fairy.MoveToPosition(honeyFlower.position); // 기본 장미 위치로 이동
        }

        Debug.Log("🧚 페어리가 장미로 이동 시작!");
    }

    /// <summary>
    /// 퍼즐 실패 처리 (씬을 재시작)
    /// </summary>
    public void FailPuzzle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }

    /// <summary>
    /// 매 프레임 호출되는 함수
    /// 페어리 이동 완료 체크, 꽃잎 떨어뜨리기 처리
    /// </summary>
    private void Update()
    {
        // 장미 도착 후 꿀 생성
        if (isMovingToHoney && !fairy.IsMoving())
        {
            SpawnHoney();      // 꿀 생성
            isMovingToHoney = false;
            return;            // 이후 로직은 실행하지 않음
        }

        // 일반 꽃 클릭 후 이동 완료 시 꽃잎 떨어뜨리기
        if (!isCleared && currentTargetFlower != null && !fairy.IsMoving())
        {
            currentTargetFlower.DropPetal(); // 꽃잎 하나 떨어뜨림
            CheckPuzzleStatus();             // 퍼즐 클리어 조건 다시 확인
            currentTargetFlower = null;      // 현재 목표 초기화
        }
    }
}





