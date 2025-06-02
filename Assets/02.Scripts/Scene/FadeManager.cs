using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    // ���̵� ó�� ��� CanvasGroup (������ Image�� �پ� �ִ� ������Ʈ)
    public CanvasGroup fadeCanvasGroup;
    // ���̵� ���� �ð�(�� ����)
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
            // ���� �� ȭ���� �˰� ���� ���·� �ΰ� ���̵����� ����
            fadeCanvasGroup.alpha = 1f;
            // ���� �߿��� ��ȣ�ۿ� ����
            fadeCanvasGroup.interactable = true;
            fadeCanvasGroup.blocksRaycasts = true;
            StartCoroutine(FadeIn());
        }
    }

    // ���̵�ƿ�: alpha�� 0 �� 1 �� ����Ǿ� ���� ȭ������ ��ȯ
    public IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        // ���� ���� ����: ��ȣ�ۿ� ���� Ȱ��ȭ
        fadeCanvasGroup.interactable = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        // ���̵�ƿ��� ���� �ڿ��� ��Ȱ��ȭ
        fadeCanvasGroup.alpha = 1f;
        fadeCanvasGroup.interactable = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    // ���̵���: alpha�� 1 �� 0 ���� ����Ǿ� ���� ȭ�鿡�� ����
    public IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null)
        {
            yield break;
        }

        // ���� ���� ����: ��ȣ�ۿ� ���� Ȱ��ȭ
        fadeCanvasGroup.interactable = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            yield return null;
        }

        // ���̵����� ���� �ڿ��� ��Ȱ��ȭ
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
        // �� �̸� ������� é�� �ε��� �Ǵ�
        if (ChapterController.Instance.TryGetChapterIndexForScene(sceneName, out int chapterIndex))
        {
            ChapterController.Instance.ShowChapterObjectOnly(chapterIndex);
        }

        yield return FadeManager.Instance.FadeOut();
        SceneManager.LoadScene(sceneName);
    }
}
