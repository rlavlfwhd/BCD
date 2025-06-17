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
            DontDestroyOnLoad(gameObject); // �ʿ� ��
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PuzzleUtils.DisableAcquiredItemObjects(); // �� �ε� �� ������ ��Ȱ��ȭ
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // �� �ε� �̺�Ʈ ����
    }

    // ���� �Ϸ� ���
    public void CompletePuzzle(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
        {
            completedPuzzles.Add(puzzleID);
            Debug.Log($"���� �Ϸ� ��ϵ�: {puzzleID}");

            if (!SceneDataManager.Instance.Data.completedPuzzles.Contains(puzzleID))
            {
                SceneDataManager.Instance.Data.completedPuzzles.Add(puzzleID);
            }
        }
    }

    // ���� �Ϸ� ���� Ȯ��
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

    // GameSystem���� ���� �� ȣ��
    public List<string> GetCompletedPuzzleList()
    {
        return new List<string>(completedPuzzles);
    }

    // GameSystem���� �ε� �� ȣ��
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
                go.SetActive(false); // �̹� ���� ������ �� ��Ȱ��ȭ
            }
            else
            {
                go.SetActive(true);  // ��ȹ�� ������ �� Ȱ��ȭ
            }
        }
    }

    private bool IsPuzzleItem(GameObject go)
    {
        return go.GetComponent<IObjectItem>() != null;
    }
}