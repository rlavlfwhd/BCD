using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PuzzleUtils.DisableAcquiredItemObjects(); // 씬 로드 후 아이템 비활성화
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 씬 로드 이벤트 해제
    }

    // 퍼즐 완료 등록
    public void CompletePuzzle(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
        {
            completedPuzzles.Add(puzzleID);
            Debug.Log($"퍼즐 완료 등록됨: {puzzleID}");

            if (!SceneDataManager.Instance.Data.completedPuzzles.Contains(puzzleID))
            {
                SceneDataManager.Instance.Data.completedPuzzles.Add(puzzleID);
            }
        }
    }

    // 퍼즐 완료 여부 확인
    public bool IsPuzzleCompleted(string puzzleID)
    {
        return completedPuzzles.Contains(puzzleID);
    }
    public void CompletePuzzleAndConsumeItem(string puzzleID, Item usedItem)
    {
        CompletePuzzle(puzzleID);
        Inventory.Instance.RemoveItemByName(usedItem.itemName);
        Inventory.Instance.ClearSelection();
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
    

    public void RestoreItemState()
    {
        var acquiredIDs = SceneDataManager.Instance.Data.acquiredItemIDs;
        GameObject[] all = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in all)
        {
            if (!IsPuzzleItem(go)) continue;

            string name = go.name.Replace("(Clone)", "").Trim();

            if (acquiredIDs.Contains(name))
            {
                go.SetActive(false); // 이미 얻은 아이템 → 비활성화
            }
            else
            {
                go.SetActive(true);  // 미획득 아이템 → 활성화
            }
        }
    }

    private bool IsPuzzleItem(GameObject go)
    {
        return go.GetComponent<IObjectItem>() != null;
    }
}