using UnityEngine;
using UnityEngine.SceneManagement;

public class MolePuzzleFailManager : MonoBehaviour
{
    public static MolePuzzleFailManager Instance;

    [Header("🎬 정답 시 이동할 씬 이름")]
    public string nextSceneName = "StoryScene"; // 성공 시 이동할 씬 이름

    [Header("📖 성공 시 StorySystem 인덱스")]
    public int nextStoryIndex = -1; // 성공 시 StorySystem에 넘길 인덱스

    [Header("🕶️ 실패 시 페이드 컨트롤러")]
    public FadeController fadeController; // 실패 시 페이드 아웃용

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 성공 처리: 스토리 인덱스 저장 후 씬 이동
    /// </summary>
    public void HandleSuccess()
    {
        Debug.Log($"🎉 정답 → StoryScene으로 이동 (StoryIndex: {nextStoryIndex})");

        // ✅ 성공 시 인덱스 저장
        if (nextStoryIndex >= 0)
        {
            SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
            Debug.Log($"📖 SceneDataManager에 nextStoryIndex 저장: {nextStoryIndex}");
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"🚪 씬 이동: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ nextSceneName이 비어있습니다 → 씬 이동 실패");
        }
    }

    /// <summary>
    /// 실패 처리: 페이드 아웃 + 실패 메시지
    /// </summary>
    public void HandleFail()
    {
        Debug.Log("💥 오답 → 페이드 아웃 + 실패 메시지 출력");

        if (fadeController != null)
        {
            fadeController.FadeOutAndRestart(); // 기존처럼 페이드 아웃 + 씬 리로드
        }
        else
        {
            Debug.LogWarning("⚠️ fadeController 없음 → 즉시 씬 리로드");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
