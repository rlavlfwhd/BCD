using UnityEngine;

public class HoneyClickHandler : MonoBehaviour
{
    [Header("📝 퍼즐 ID와 다음 스토리 인덱스")]
    public string puzzleID;
    public int nextStoryIndex = -1;

    [Header("🎯 결과 이미지와 스프라이트")]
    public UnityEngine.UI.Image resultImage;
    public Sprite successSprite;

    private void OnMouseDown()
    {
        Debug.Log("🍯 꿀 클릭됨! PuzzleManager에 HandlePuzzleSuccess 호출 준비");

        if (PuzzleManager.Instance != null)
        {
            PuzzleManager.Instance.HandlePuzzleSuccess(resultImage, successSprite, nextStoryIndex, puzzleID);
            Debug.Log($"✅ PuzzleManager.HandlePuzzleSuccess 호출됨 (puzzleID: {puzzleID}, nextStoryIndex: {nextStoryIndex})");
        }
        else
        {
            Debug.LogWarning("⚠️ PuzzleManager 인스턴스를 찾지 못했습니다!");
        }
    }
}
