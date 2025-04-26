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

    private void Start()
    {
        //RestoreItemState();
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
        StorySystem.Instance.StoryShow(storyIndex);
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

    public void CompletePuzzleAndConsumeItem(string puzzleID, Item usedItem)
    {
        CompletePuzzle(puzzleID);
        Inventory.Instance.RemoveItemByName(usedItem.itemName);
        Inventory.Instance.ClearSelection();
    }
}