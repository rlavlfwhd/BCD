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
            DontDestroyOnLoad(gameObject); // �ʿ� ��
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RestoreItemState();
    }

    // ���� �Ϸ� ���
    public void CompletePuzzle(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
        {
            completedPuzzles.Add(puzzleID);
            Debug.Log($"���� �Ϸ� ��ϵ�: {puzzleID}");
        }
    }

    // ���� �Ϸ� ���� Ȯ��
    public bool IsPuzzleCompleted(string puzzleID)
    {
        return completedPuzzles.Contains(puzzleID);
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

    // ���� Ŭ���� �� �ļ� �ൿ���� ó�� (�̹��� ���� + ���� ���丮�� �̵�)
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

    private void RestoreItemState()
{
    var acquiredIDs = SceneDataManager.Instance.Data.acquiredItemIDs;
    GameObject[] all = GameObject.FindObjectsOfType<GameObject>(true);

    foreach (GameObject go in all)
    {
        if (!IsPuzzleItem(go)) continue;

        string name = go.name.Replace("(Clone)", "").Trim();

        if (acquiredIDs.Contains(name))
        {
            go.SetActive(false); // �̹� ���� �������̸� ����
        }
        else
        {
            go.SetActive(true);  // ���� �� ���� �������̸� ���̰�
        }
    }
}

private bool IsPuzzleItem(GameObject go)
{
    return go.GetComponent<IObjectItem>() != null;
}
}
