using System.Collections;
using UnityEngine;

public class ClickNextStoryManager : MonoBehaviour, IClickablePuzzle
{
    [Header("이동할 스토리 번호")]
    public int storyIndex = 400;

    private bool isClicked = false;

    public void OnClickPuzzle()
    {
        if (isClicked) return;

        isClicked = true;

        Debug.Log($"[ClickNextStoryManager] 오브젝트 클릭됨! 스토리 {storyIndex}로 이동");

        // 다음 스토리 번호 저장
        SceneDataManager.Instance.Data.nextStoryIndex = storyIndex;

        // 페이드 후 씬 전환
        StartCoroutine(FadeAndLoadStoryScene());
    }

    private IEnumerator FadeAndLoadStoryScene()
    {
        yield return FadeManager.Instance.FadeToStoryScene("StoryScene");
    }
}
