using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ⭐ 반드시 필요

public class FlowerPuzzleController : MonoBehaviour
{
    [Header("🌸 퍼즐에 사용될 4개의 꽃 오브젝트")]
    public FlowerController flower1;
    public FlowerController flower2;
    public FlowerController flower3;
    public FlowerController flower4;

    [Header("🎯 각 꽃이 맞춰야 할 정답 꽃잎 개수")]
    public int targetPetalCount1;
    public int targetPetalCount2;
    public int targetPetalCount3;
    public int targetPetalCount4;

    [Header("🧚 페어리 이동 및 횟수 제한")]
    public FairyController fairy;
    public int moveLimit = 5;
    private int currentMoveCount = 0;
    private FlowerController currentTargetFlower;

    [Header("🍯 퍼즐 성공 시 꿀 생성")]
    public GameObject honeyPrefab;
    public Transform honeyFlower;
    public Transform honeyArrivalPoint;
    public Transform honeySpawnPoint;

    [Header("📽 페이드 연출 컨트롤러")]
    public FadeController fadeController;

    [Header("🛫 퍼즐 완료 후 이동할 씬 이름")]
    public string nextSceneName = "StoryScene";

    [Header("📝 퍼즐 완료 후 보여줄 스토리 인덱스")]
    public int nextStoryIndex = 1;

    [Header("🔀 스토리 인덱스 사용 여부")]
    public bool useStoryIndex = true;

    private bool isCleared = false;
    private bool isMovingToHoney = false;

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
            PuzzleManager.Instance.CompletePuzzle("FlowerPuzzle");

            MoveFairyToHoney();
        }
    }

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

    private void Update()
    {
        if (isMovingToHoney && !fairy.IsMoving())
        {
            SpawnHoney();
            isMovingToHoney = false;

            Debug.Log("🌸 퍼즐 클리어 → 2초 뒤 스토리 이동 시작!");
            StartCoroutine(DelayAndShowStory());
        }

        if (!isCleared && currentTargetFlower != null && !fairy.IsMoving())
        {
            currentTargetFlower.DropPetal();
            CheckPuzzleStatus();
            currentTargetFlower = null;
        }
    }

    private void SpawnHoney()
    {
        if (honeyPrefab != null && honeySpawnPoint != null)
        {
            Instantiate(honeyPrefab, honeySpawnPoint.position, Quaternion.identity);
            Debug.Log("🍯 꿀 생성 완료!");
        }
    }

    private IEnumerator DelayAndShowStory()
    {
        yield return new WaitForSeconds(2f);

        if (useStoryIndex)
        {
            Debug.Log($"🚪 씬 이동: {nextSceneName}, 스토리 인덱스: {nextStoryIndex}");
            SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
            StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));
        }
        else
        {
            Debug.Log($"🚪 씬 이동: {nextSceneName} (스토리 인덱스 없이)");
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void FailPuzzle()
    {
        if (fadeController != null)
        {
            if (!fadeController.gameObject.activeInHierarchy)
                fadeController.gameObject.SetActive(true);

            fadeController.FadeOutAndRestart();
        }
        else
        {
            Debug.LogWarning("⚠️ FadeController 없음. 씬 재시작");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public bool IsPuzzleCleared()
    {
        return isCleared;
    }

    public bool IsFairyMoving()
    {
        return fairy != null && fairy.IsMoving();
    }
}
