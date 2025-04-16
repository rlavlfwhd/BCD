using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpenedWindowTriggerUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject overlayImage; // ��ü ȭ�� ���� �̹���
    public float waitSeconds = 2f;
    public string storySceneName = "PlayScene";
    public int startStoryNumber = 11;
    public string puzzleID = "window_rope";
    private bool triggered = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (triggered) return;
        if (!PuzzleManager.Instance.IsPuzzleCompleted(puzzleID)) return;

        triggered = true;
        overlayImage.SetActive(true);
        StartCoroutine(WaitAndGoToStory());
    }

    private IEnumerator WaitAndGoToStory()
    {
        yield return new WaitForSeconds(waitSeconds);
        SceneManager.LoadScene(storySceneName);
        yield return null;
        GameSystem.Instance.currentStoryIndex = startStoryNumber;
        GameSystem.Instance.StoryShow(startStoryNumber);
        Debug.Log("�� ��ȯ �� ������ ���丮 �ε���: " + GameSystem.Instance.currentStoryIndex);
    }
}