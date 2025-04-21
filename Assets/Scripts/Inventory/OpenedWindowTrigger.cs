using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpenedWindowTriggerUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject overlayImage; // 전체 화면 덮는 이미지
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

        SceneDataManager.Instance.Data.nextStoryIndex = startStoryNumber;
        SceneManager.LoadScene(storySceneName);
    }
}