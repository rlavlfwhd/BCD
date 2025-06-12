using UnityEngine;

/// <summary>
/// 오브젝트를 클릭하면 지정된 스토리 번호로 이동하는 간단한 매니저
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ClickNextStoryManager : MonoBehaviour
{
    [Header("📖 이동할 스토리 번호")]
    public int storyIndex = 0;

    private bool isClicked = false;

    private void OnMouseDown()
    {
        if (isClicked) return;

        isClicked = true;

        Debug.Log($"🖱️ 오브젝트 클릭됨! 스토리 {storyIndex}로 이동");

        // 스토리 번호 저장
        SceneDataManager.Instance.Data.nextStoryIndex = storyIndex;

        // 씬 전환
        UnityEngine.SceneManagement.SceneManager.LoadScene("StoryScene");
    }
}
