using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleTileManager : MonoBehaviour
{
    [Header("🔲 퍼즐 타일 목록")]
    public List<PuzzleTile> tiles = new List<PuzzleTile>();

    [Header("✅ 정답 패턴 (타일 개수와 동일)")]
    public List<bool> answerPattern = new List<bool>();

    [Header("🌄 퍼즐 클리어 시 교체할 이미지")]
    public Image resultImage;
    public Sprite successSprite;

    [Header("🧩 퍼즐 고유 ID / 다음 스토리 인덱스")]
    public string puzzleID;
    public int nextStoryIndex;

    private bool puzzleCleared = false;

    void Update()
    {
        // 퍼즐이 아직 클리어되지 않았고 정답과 일치하면
        if (!puzzleCleared && CheckAnswer())
        {
            puzzleCleared = true;

            // 🎯 디버그 출력
            Debug.Log("🎉 퍼즐 클리어!");

            // 퍼즐 완료 처리
            PuzzleManager.Instance?.CompletePuzzle(puzzleID);
            PuzzleManager.Instance?.HandlePuzzleSuccess(resultImage, successSprite, nextStoryIndex, puzzleID);
        }
    }

    /// <summary>
    /// 현재 타일 상태가 정답과 일치하는지 확인
    /// </summary>
    /// <returns>정답이면 true</returns>
    private bool CheckAnswer()
    {
        // 길이 일치 안 하면 false
        if (tiles.Count != answerPattern.Count)
        {
            Debug.LogWarning("⚠️ 타일 수와 정답 패턴 수가 다릅니다!");
            return false;
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].isOn != answerPattern[i])
                return false;
        }

        return true;
    }
}


