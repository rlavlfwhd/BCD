using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinePuzzleManager : MonoBehaviour
{
    [Header("✅ 쉐이커 컨트롤러")]
    public ShakeController shakeController;

    [Header("📖 퍼즐 성공 시 이동할 스토리 번호")]
    public int successStoryIndex = 0;

    [Header("📖 퍼즐 실패 시 이동할 스토리 번호")]
    public int failureStoryIndex = 0;

    [Header("🖼️ 성공/실패 오버레이 이미지")]
    public GameObject successOverlayImage;
    public GameObject failureOverlayImage;

    [Header("🎯 와인 선택 횟수 제한")]
    public int maxTries = 5;
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
            StartCoroutine(HandleResultSequence(true)); // 성공
        }
        else
        {
            Debug.Log("🍷 틀린 순서입니다. 이상한 와인을 생성합니다.");
            StartCoroutine(HandleResultSequence(false)); // 실패
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
        }
        else
        {
            isWeirdWineCreated = true;
        }

        selectedWineOrder.Clear();
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
            StartCoroutine(GoToStoryAfterDelay(successStoryIndex, successOverlayImage));
        }
        else if (isWeirdWineCreated)
        {
            StartCoroutine(GoToStoryAfterDelay(failureStoryIndex, failureOverlayImage));
        }
    }

    private IEnumerator GoToStoryAfterDelay(int storyIndex, GameObject overlayObj)
    {
        if (overlayObj != null)
        {
            overlayObj.SetActive(true);
            SpriteRenderer overlay = overlayObj.GetComponent<SpriteRenderer>();
            if (overlay != null)
            {
                Color color = overlay.color;
                color.a = 0f;
                overlay.color = color;

                float timer = 0f;
                float fadeDuration = 1f;

                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                    overlay.color = color;
                    yield return null;
                }

                color.a = 1f;
                overlay.color = color;
            }
        }

        yield return new WaitForSeconds(1f); // 추가 지연 시간

        SceneDataManager.Instance.Data.nextStoryIndex = successStoryIndex;
        StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));
    }

    public bool IsPuzzleCompleted() => isPuzzleCompleted;
    public bool IsWeirdWineCreated() => isWeirdWineCreated;
}
