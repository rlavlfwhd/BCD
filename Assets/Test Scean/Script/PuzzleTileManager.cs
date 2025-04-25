using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTileManager : MonoBehaviour
{
    public static PuzzleTileManager Instance;

    [Header("퍼즐 타일 목록")]
    public List<PuzzleTile> tiles = new List<PuzzleTile>();

    [Header("정답 패턴 (타일 개수와 동일)")]
    public List<bool> answerPattern = new List<bool>();

    [Header("퍼즐 성공 시 이미지 전환 대상")]
    public Image resultImage;
    public Sprite successSprite;

    [Header("퍼즐 고유 ID (저장용)")]
    public string puzzleID = "MyPuzzle";

    [Header("성공 시 다음 스토리 인덱스")]
    public int nextStoryIndex = 0;

    private bool puzzleCleared = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // 이미 성공했거나 PuzzleManager가 없으면 패스
        if (puzzleCleared || PuzzleManager.Instance == null) return;

        // 이미 완료된 퍼즐이라면 패스
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID)) return;

        // 퍼즐 정답 확인
        if (IsPuzzleCorrect())
        {
            HandlePuzzleSuccess();
        }
    }

    /// <summary>
    /// 정답 타일에만 작물이 올라가 있는지 검사
    /// </summary>
    public bool IsPuzzleCorrect()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (answerPattern[i]) // 정답 위치라면
            {
                if (!tiles[i].isOn) return false; // 정답 위치에 작물이 없으면 실패
            }
        }
        return true;
    }

    /// <summary>
    /// 퍼즐 성공 처리 (PuzzleManager에 등록 + 이미지 변경 + 스토리 이동)
    /// </summary>
    private void HandlePuzzleSuccess()
    {
        Debug.Log("🎉 퍼즐 정답 성공!");

        puzzleCleared = true;

        // 퍼즐 클리어 등록 (중복 방지)
        PuzzleManager.Instance.CompletePuzzle(puzzleID);

        // 이미지 전환
        if (resultImage != null && successSprite != null)
            resultImage.sprite = successSprite;

        // 다음 스토리 진행
        StartCoroutine(GoToNextStory(nextStoryIndex));
    }

    private System.Collections.IEnumerator GoToNextStory(int storyIndex)
    {
        yield return new WaitForSeconds(1.0f);
        StorySystem.Instance.StoryShow(storyIndex);
    }

    /// <summary>
    /// 퍼즐 타일의 인덱스를 반환
    /// </summary>
    public int GetTileIndex(PuzzleTile tile)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] == tile)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// 특정 인덱스가 정답 타일인지 확인
    /// </summary>
    public bool IsCorrectTile(int index)
    {
        if (index < 0 || index >= answerPattern.Count) return false;
        return answerPattern[index];
    }
}

