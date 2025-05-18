using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChapterController : MonoBehaviour
{
    public static ChapterController Instance;

    public GameObject[] chapters;
    public GameObject activeChapter;
    public System.Action OnChapterFinished;

    //  ∆€¡Òæ¿ ¿Ã∏ß °Ê √©≈Õ ¿Œµ¶Ω∫ ∏≈«Œ
    private Dictionary<string, int> puzzleSceneChapterMap = new Dictionary<string, int>()
    {        
        { "PBookshelfScene", 8 },
        { "PWindowScene", 9 }
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // æ¿ ¿¸»Ø Ω√ ¿Ø¡ˆ
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[ChapterController] æ¿ ∑ŒµÂµ : {scene.name}");

        string sceneName = scene.name;

        if (activeChapter != null)
        {
            activeChapter.SetActive(false);
            activeChapter = null;
        }

        if (puzzleSceneChapterMap.TryGetValue(sceneName, out int chapterIndex))
        {
            Debug.Log($"[ChapterController] ∆€¡Òæ¿ ∞®¡ˆµ : {sceneName} °Ê √©≈Õ {chapterIndex}");
            ChangeChapter(chapterIndex);
        }
        else
        {
            Debug.Log($"[ChapterController] ∆€¡Òæ¿¿Ã¡ˆ∏∏ √©≈Õ ∏≈«Œ æ¯¿Ω: {sceneName}");
        }
    }

    public static int GetChapterIndexForStoryNumber(int number)
    {
        if (number >= 50) return 2;
        if (number >= 20) return 1;
        return 0;
    }

    public void ChangeChapter(int chapterIndex)
    {
        if (activeChapter != null)
        {
            PlayableDirector prevDirector = activeChapter.GetComponent<PlayableDirector>();
            if (prevDirector != null)
                prevDirector.Stop();

            activeChapter.SetActive(false);
        }

        if (chapterIndex < 0 || chapters == null || chapterIndex >= chapters.Length)
        {
            Debug.LogWarning($"[ChapterController] ¿Ø»ø«œ¡ˆ æ ¿∫ √©≈Õ ¿Œµ¶Ω∫: {chapterIndex}");
            return;
        }

        activeChapter = chapters[chapterIndex];
        activeChapter.SetActive(true);

        PlayableDirector newDirector = activeChapter.GetComponent<PlayableDirector>();
        if (newDirector != null)
        {
            newDirector.playOnAwake = false;
            newDirector.Play();
            StartCoroutine(DisableChapterAfterTimeline(newDirector));
        }
        else
        {
            StartCoroutine(DisableChapterAfterSeconds(2.0f));
        }
    }

    private IEnumerator DisableChapterAfterTimeline(PlayableDirector director)
    {
        yield return new WaitForSeconds((float)director.duration);

        if (activeChapter != null)
            activeChapter.SetActive(false);

        if (OnChapterFinished != null)
        {
            OnChapterFinished();
        }
    }

    private IEnumerator DisableChapterAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (activeChapter != null)
        {
            activeChapter.SetActive(false);
        }
    }
}
