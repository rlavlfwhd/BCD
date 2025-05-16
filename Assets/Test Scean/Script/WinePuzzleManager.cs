using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinePuzzleManager : MonoBehaviour
{
    [Header("✅ 쉐이커 컨트롤러")]
    public ShakeController shakeController;

    [Header("📖 퍼즐 완료 후 이동할 스토리 번호")]
    public int nextStoryIndex = 0;

    [Header("🖼️ 오버레이 페이드 이미지")]
    public GameObject overlayImage;

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
            StartCoroutine(HandleSuccessSequence());
        }
        else
        {
            Debug.Log("🍷 순서가 틀렸습니다. 이상한 와인을 생성합니다.");
            StartCoroutine(HandleWeirdWineSequence());
        }
    }

    private IEnumerator HandleSuccessSequence()
    {
        if (shakeController != null)
        {
            yield return StartCoroutine(shakeController.StartShaking());
        }

        isPuzzleCompleted = true;
        selectedWineOrder.Clear();
    }

    private IEnumerator HandleWeirdWineSequence()
    {
        isWeirdWineCreated = true;

        // 👉 여기서 이상한 와인 연출 넣기
        Debug.Log("🧪 이상한 와인이 만들어졌습니다!");

        // TODO: 이상한 와인 이펙트, 사운드 등 넣을 수 있음

        yield return null;
    }

    public void ResetTries()
    {
        currentTries = 0;
        selectedWineOrder.Clear();
        isPuzzleCompleted = false;
        isWeirdWineCreated = false;
        Debug.Log("🔄 모든 상태 초기화 완료");
    }

    public void TryGoToStory()
    {
        if (isPuzzleCompleted || isWeirdWineCreated)
        {
            StartCoroutine(GoToStoryAfterDelay(2f));
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        if (overlayImage != null)
        {
            overlayImage.SetActive(true);
            SpriteRenderer overlay = overlayImage.GetComponent<SpriteRenderer>();
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

        yield return new WaitForSeconds(delay);

        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }

    public bool IsPuzzleCompleted()
    {
        return isPuzzleCompleted;
    }

    public bool IsWeirdWineCreated()
    {
        return isWeirdWineCreated;
    }
}