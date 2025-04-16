using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    private HashSet<string> completedPuzzles = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 필요 시
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 퍼즐 완료 등록
    public void CompletePuzzle(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
        {
            completedPuzzles.Add(puzzleID);
            Debug.Log($"퍼즐 완료 등록됨: {puzzleID}");
        }
    }

    // 퍼즐 완료 여부 확인
    public bool IsPuzzleCompleted(string puzzleID)
    {
        return completedPuzzles.Contains(puzzleID);
    }

    // GameSystem에서 저장 시 호출
    public List<string> GetCompletedPuzzleList()
    {
        return new List<string>(completedPuzzles);
    }

    // GameSystem에서 로드 시 호출
    public void SetCompletedPuzzleList(List<string> list)
    {
        completedPuzzles = new HashSet<string>(list);
    }

    // 퍼즐 클리어 시 후속 행동까지 처리 (이미지 변경 + 다음 스토리로 이동)
    public void HandlePuzzleSuccess(Image targetImage, Sprite newSprite, int nextStoryIndex, string puzzleID)
    {
        if (IsPuzzleCompleted(puzzleID)) return;

        CompletePuzzle(puzzleID);

        if (targetImage != null && newSprite != null)
            targetImage.sprite = newSprite;

        StartCoroutine(GoToNextStory(nextStoryIndex));
    }

    private IEnumerator GoToNextStory(int storyIndex)
    {
        yield return new WaitForSeconds(2f);
        GameSystem.Instance.StoryShow(storyIndex);
    }
}
