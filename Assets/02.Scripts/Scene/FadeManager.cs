using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    // 페이드 처리 대상 CanvasGroup (검은색 Image가 붙어 있는 오브젝트)
    public CanvasGroup fadeCanvasGroup;
    // 페이드 지속 시간(초 단위)
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (fadeCanvasGroup != null)
        {
            // 시작 시 화면을 검게 덮은 상태로 두고 페이드인을 실행
            fadeCanvasGroup.alpha = 1f;
            // 연출 중에는 상호작용 차단
            fadeCanvasGroup.interactable = true;
            fadeCanvasGroup.blocksRaycasts = true;
            StartCoroutine(FadeIn());
        }
    }

    // 페이드아웃: alpha가 0 → 1 로 변경되어 검은 화면으로 전환
    public IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        // 연출 시작 시점: 상호작용 차단 활성화
        fadeCanvasGroup.interactable = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        // 페이드아웃이 끝난 뒤에는 비활성화
        fadeCanvasGroup.alpha = 1f;
        fadeCanvasGroup.interactable = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    // 페이드인: alpha가 1 → 0 으로 변경되어 검은 화면에서 해제
    public IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        // 연출 시작 시점: 상호작용 차단 활성화
        fadeCanvasGroup.interactable = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        // 페이드인이 끝난 뒤에는 비활성화
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.interactable = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeToStoryScene(string sceneName)
    {
        int nextStoryIndex = SceneDataManager.Instance.Data.nextStoryIndex;
        int chapterIndex = ChapterController.GetChapterIndexForStoryNumber(nextStoryIndex);

        ChapterController.Instance.ShowChapterObjectOnly(chapterIndex);

        yield return FadeManager.Instance.FadeOut();
        SceneManager.LoadScene(sceneName);
    }

    public IEnumerator FadeToChoiceScene(string sceneName)
    {
        // 씬 이름 기반으로 챕터 인덱스 판단
        if (ChapterController.Instance.TryGetChapterIndexForScene(sceneName, out int chapterIndex))
        {
            ChapterController.Instance.ShowChapterObjectOnly(chapterIndex);
        }

        yield return FadeManager.Instance.FadeOut();
        SceneManager.LoadScene(sceneName);
    }
}
