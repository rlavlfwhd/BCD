using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // ⭐ 추가: 씬 이동을 위해 필요

[System.Serializable]
public class PuzzleSet
{
    public List<PuzzleTile> tiles = new List<PuzzleTile>(); // 퍼즐 타일 목록
    public List<bool> answerPattern = new List<bool>();     // 정답 패턴
    public string puzzleID;                                 // 퍼즐 ID
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
    public GameObject clearImageCanvas; // 퍼즐 클리어시 보여줄 캔버스
    public float showDuration = 1.5f;   // 클리어 연출 지속 시간 (초)

    [Header("🛫 퍼즐 완료 후 이동할 씬 이름")] // ⭐ Inspector에서 입력 가능하게 설정
    public string nextSceneName;

    private bool isPuzzleCleared = false;

    private void Update()
    {
        // 이미 퍼즐 클리어했다면 더 이상 체크 안 함
        if (isPuzzleCleared) return;

        // 타일 수와 정답 패턴 수가 다르면 체크 안 함
        if (tiles.Count != answerPattern.Count) return;

        // 모든 타일이 정답과 일치하는지 검사
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

        // 퍼즐 성공 처리
        if (isCorrect)
        {
            HandlePuzzleSuccess();
        }
    }

    private void HandlePuzzleSuccess()
    {
        Debug.Log($"✅ 퍼즐 클리어 성공! 퍼즐 ID: {puzzleID}");

        isPuzzleCleared = true;

        // 결과 이미지 변경
        if (resultImage != null && successSprite != null)
        {
            resultImage.sprite = successSprite;
        }

        // 퍼즐 완료 등록
        PuzzleManager.Instance.CompletePuzzle(puzzleID);

        // 퍼즐 세트 전환 처리 시작
        StartCoroutine(GoToNextPuzzleDelayed());

        // 클리어 이미지 띄우기 (동시에 실행)
        if (clearImageCanvas != null)
        {
            StartCoroutine(ShowClearImageOnly());
        }
    }

    private IEnumerator ShowClearImageOnly()
    {
        clearImageCanvas.SetActive(true);             // 클리어 연출 켜기
        yield return new WaitForSeconds(showDuration); // 설정 시간만큼 대기
        clearImageCanvas.SetActive(false);             // 연출 끄기
    }

    private IEnumerator GoToNextPuzzleDelayed()
    {
        yield return new WaitForSeconds(0.1f); // 아주 짧게 대기

        // 현재 퍼즐 세트 비활성화
        DeactivateCurrentPuzzleSet();

        // 다음 퍼즐 세트 찾기
        PuzzleSet nextSet = FindNextPuzzleSet();
        if (nextSet != null)
        {
            Debug.Log($"➡️ 다음 퍼즐로 이동! 새로운 퍼즐 ID: {nextSet.puzzleID}");

            // 다음 퍼즐 세트 정보 갱신
            tiles = nextSet.tiles;
            answerPattern = nextSet.answerPattern;
            puzzleID = nextSet.puzzleID;
            isPuzzleCleared = false;

            // 퍼즐 세트 활성화
            ActivatePuzzleSet(nextSet.puzzleID);
        }
        else
        {
            Debug.Log("🎉 모든 퍼즐 완료!");

            // 🔥 퍼즐 다 풀었으면 원하는 씬으로 이동 시작
            yield return new WaitForSeconds(1f); // 1초 정도 여유를 준 후 이동

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log($"🚪 다음 씬으로 이동: {nextSceneName}");
                SceneManager.LoadScene(nextSceneName); // ⭐ Inspector에 입력한 씬으로 이동
            }
            else
            {
                Debug.LogWarning("⚠️ 이동할 씬 이름이 비어 있습니다!");
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







