using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PuzzleSet
{
    public List<PuzzleTile> tiles = new List<PuzzleTile>();
    public List<bool> answerPattern = new List<bool>();
    public string puzzleID;
}

public class PuzzleTileManager : MonoBehaviour
{
    [Header("현재 퍼즐 진행용 타일들")]
    public List<PuzzleTile> tiles = new List<PuzzleTile>();

    [Header("현재 퍼즐 정답 패턴")]
    public List<bool> answerPattern = new List<bool>();

    [Header("퍼즐 클리어 후 이미지 교체 (선택)")]
    public Image resultImage;
    public Sprite successSprite;

    [Header("퍼즐 ID / 다음 스토리 인덱스")]
    public string puzzleID;
    public int nextStoryIndex = -1; // -1이면 스토리 이동 없음

    [Header("퍼즐 세트 목록")]
    public List<PuzzleSet> puzzleSets = new List<PuzzleSet>();

    [Header("퍼즐 세트 오브젝트 목록 (PuzzleSet_1, PuzzleSet_2...)")]
    public List<GameObject> puzzleSetObjects = new List<GameObject>();

    [Header("퍼즐 클리어 연출 (추가)")]
    public GameObject clearImageCanvas;
    public float showDuration = 1.5f;

    [Header("🛫 퍼즐 완료 후 이동할 씬 이름")]
    public string nextSceneName;

    private bool isPuzzleCleared = false;
    

    private void Update()
    {
        if (isPuzzleCleared) return;
        if (tiles.Count != answerPattern.Count) return;

        bool isCorrect = true;
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i] == null) continue;
            if (tiles[i].isOn != answerPattern[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            HandlePuzzleSuccess();
        }
    }

    private void HandlePuzzleSuccess()
    {
        Debug.Log($"✅ 퍼즐 클리어 성공! 퍼즐 ID: {puzzleID}");

        isPuzzleCleared = true;

        if (resultImage != null && successSprite != null)
        {
            resultImage.sprite = successSprite;
        }

        PuzzleManager.Instance.CompletePuzzle(puzzleID);
        StartCoroutine(GoToNextPuzzleDelayed());

        if (clearImageCanvas != null)
        {
            StartCoroutine(ShowClearImageOnly());
        }
    }

    private IEnumerator ShowClearImageOnly()
    {
        clearImageCanvas.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        clearImageCanvas.SetActive(false);
    }

    private IEnumerator GoToNextPuzzleDelayed()
    {
        yield return new WaitForSeconds(0.1f);

        DeactivateCurrentPuzzleSet();

        PuzzleSet nextSet = FindNextPuzzleSet();
        if (nextSet != null)
        {
            Debug.Log($"➡️ 다음 퍼즐로 이동! 새로운 퍼즐 ID: {nextSet.puzzleID}");

            tiles = nextSet.tiles;
            answerPattern = nextSet.answerPattern;
            puzzleID = nextSet.puzzleID;
            isPuzzleCleared = false;

            ActivatePuzzleSet(nextSet.puzzleID);
        }
        else
        {
            Debug.Log("🎉 모든 퍼즐 완료!");
            yield return new WaitForSeconds(1f);

            // ✅ 스토리 인덱스가 설정된 경우 우선 처리
            if (nextStoryIndex >= 0)
            {
                Debug.Log($"📦 nextStoryIndex({nextStoryIndex}) 저장 후 StoryScene으로 이동");
                SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
                StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));
            }
            else if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log($"🚪 다음 씬으로 이동: {nextSceneName}");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("⚠️ 이동할 씬 이름과 스토리 인덱스가 모두 비어 있습니다!");
            }
        }
    }

    private void DeactivateCurrentPuzzleSet()
    {
        foreach (GameObject obj in puzzleSetObjects)
        {
            if (obj != null && obj.activeSelf)
            {
                obj.SetActive(false);
                Debug.Log($"🚫 퍼즐 세트 비활성화: {obj.name}");
            }
        }
    }

    private PuzzleSet FindNextPuzzleSet()
    {
        for (int i = 0; i < puzzleSets.Count; i++)
        {
            if (puzzleSets[i].puzzleID == puzzleID && i + 1 < puzzleSets.Count)
            {
                return puzzleSets[i + 1];
            }
        }
        return null;
    }

    private void ActivatePuzzleSet(string targetPuzzleID)
    {
        foreach (GameObject obj in puzzleSetObjects)
        {
            if (obj != null && obj.name.Contains(targetPuzzleID))
            {
                obj.SetActive(true);
                Debug.Log($"🟢 퍼즐 세트 활성화: {obj.name}");
            }
        }
    }
}
