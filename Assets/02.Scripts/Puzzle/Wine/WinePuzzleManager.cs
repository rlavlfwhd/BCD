using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinePuzzleManager : MonoBehaviour
{
    [Header("✅ 쉐이커 컨트롤러")] public ShakeController shakeController;

    [Header("📖 퍼즐 성공 시 이동할 스토리 번호")] public int successStoryIndex = 180;
    [Header("📖 퍼즐 실패 시 이동할 스토리 번호")] public int failureStoryIndex = 190;


    [Header("🎯 와인 선택 횟수 제한")] public int maxTries = 5;
    private int currentTries = 0;

    private readonly List<string> correctWineOrder = new List<string>
    {
        "Green", "Yellow", "Orange", "Red", "Blue"
    };

    private List<string> selectedWineOrder = new List<string>();
    private bool isPuzzleCompleted = false;
    private bool isWeirdWineCreated = false;

    public void SelectWine(string wineColor)
    {
        if (currentTries >= maxTries || isPuzzleCompleted || isWeirdWineCreated)
        {
            Debug.Log("🚫 퍼즐 입력 불가 상태입니다.");
            return;
        }

        selectedWineOrder.Add(wineColor);
        currentTries++;

        Debug.Log($"📦 현재 선택 순서: {string.Join(", ", selectedWineOrder)} / 시도 {currentTries}/{maxTries}");

        if (currentTries >= maxTries)
        {
            Debug.Log("🛑 최대 시도 도달, 병 클릭 비활성화");
            LockAllWineBottles();
        }

        if (selectedWineOrder.Count == correctWineOrder.Count)
        {
            CheckSequence();
        }
    }

    private void CheckSequence()
    {
        bool isCorrect = true;

        for (int i = 0; i < correctWineOrder.Count; i++)
        {
            if (selectedWineOrder[i] != correctWineOrder[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("🎉 퍼즐 정답! 연출 시작");
            StartCoroutine(HandleResultSequence(true));
        }
        else
        {
            Debug.Log("🍷 틀린 순서입니다. 이상한 와인을 생성합니다.");
            StartCoroutine(HandleResultSequence(false));
        }
    }

    private IEnumerator HandleResultSequence(bool isSuccess)
    {
        if (shakeController != null)
        {
            yield return StartCoroutine(shakeController.StartShaking(isSuccess));
        }

        if (isSuccess)
        {
            isPuzzleCompleted = true;

            Inventory.Instance.RemoveItemByName("Hint_Fake");
            Inventory.Instance.RemoveItemByName("Hint");
        }
        else
        {
            isWeirdWineCreated = true;
        }
        selectedWineOrder.Clear();
    }

    private void LockAllWineBottles()
    {
        WineBottle[] bottles = FindObjectsOfType<WineBottle>();
        foreach (var bottle in bottles)
        {
            bottle.LockBottle();
        }
    }

    public void ResetTries()
    {
        currentTries = 0;
        selectedWineOrder.Clear();
        isPuzzleCompleted = false;
        isWeirdWineCreated = false;
        Debug.Log("🔄 상태 초기화 완료");
    }

    public void TryGoToStory()
    {
        if (isPuzzleCompleted)
        {
            StartCoroutine(GoToStoryAfterDelay(successStoryIndex));
        }
        else if (isWeirdWineCreated)
        {
            StartCoroutine(GoToStoryAfterDelay(failureStoryIndex));
        }
    }

    private IEnumerator GoToStoryAfterDelay(int storyIndex)
    {
        yield return new WaitForSeconds(0.2f);
        SceneDataManager.Instance.Data.nextStoryIndex = storyIndex;
        StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));
    }

    public bool IsPuzzleCompleted() => isPuzzleCompleted;
    public bool IsWeirdWineCreated() => isWeirdWineCreated;
}
