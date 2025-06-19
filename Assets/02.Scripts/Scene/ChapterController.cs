using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterController : MonoBehaviour
{
    public static ChapterController Instance;

    public static bool skipChapterOnLoad = false;

    // 각 챕터용 GameObject 배열 (기존에는 PlayableDirector를 포함한 Timeline 오브젝트였으나, 이제는 단순 GameObject로 처리)
    public GameObject[] chapters;
    // 현재 활성화된 챕터 GameObject
    public GameObject activeChapter;
    // 챕터 연출이 완료되었을 때 호출될 콜백
    public System.Action OnChapterFinished;

    // 퍼즐 씬 이름과 매칭되는 챕터 인덱스를 정의 (필요에 따라 수정 가능)
    private Dictionary<string, int> puzzleSceneChapterMap = new Dictionary<string, int>()
    {
        { "PGardenScene", 4 },
        { "PWineScene", 5 },
        { "PFlowerScene", 6 },
        { "PMoleScene", 7 },
        { "PBookshelfScene", 8 },
        { "PWindowScene", 9 },
        { "PortraitScene", 10 },
        { "PQueenScene", 11 },
        { "HappyEnding", 12 },
        { "BadEnding", 13 },
        { "PrincessRoom", 14 },
        { "PMix", 15 },
        { "PMix2", 16 }


    };

    private void Awake()
    {
        // 싱글톤 패턴 설정
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
        if (skipChapterOnLoad)
        {
            skipChapterOnLoad = false;
            return;
        }
        string sceneName = scene.name;

        if (puzzleSceneChapterMap.TryGetValue(sceneName, out int chapterIndex))
        {
            ChangeChapter(chapterIndex);
        }
    }

   
    public bool TryGetChapterIndexForScene(string sceneName, out int chapterIndex)
    {
        return puzzleSceneChapterMap.TryGetValue(sceneName, out chapterIndex);
    }

    // 스토리 번호에 따라 챕터 인덱스를 반환 (필요 시 조정)
    public static int GetChapterIndexForStoryNumber(int number)
    {
        if (number >= 99)
        {
            return 2;
        }
        if (number >= 40)
        {
            return 1;
        }
        return 0;
    }
   

    // 챕터를 전환하고 페이드인 연출만 실행
    public void ChangeChapter(int chapterIndex)
    {
        var go = ShowChapterObjectOnly(chapterIndex);
        if (go != null)
            StartCoroutine(ChapterFadeFlow(go));
    }
    public GameObject ShowChapterObjectOnly(int chapterIndex)
    {
        if (activeChapter != null)
        {
            // 챕터 배열 범위 체크
            if (chapterIndex >= 0 && chapterIndex < chapters.Length)
            {
                if (activeChapter == chapters[chapterIndex] && activeChapter.activeSelf)
                    return activeChapter;
            }
            // 현재 activeChapter가 지금 켜려는 것과 다르다면 기존 것만 끄고, 새로 킴
            activeChapter.SetActive(false);
        }

        if (chapters == null || chapterIndex < 0 || chapterIndex >= chapters.Length)
            return null;

        activeChapter = chapters[chapterIndex];
        activeChapter.SetActive(true);
        return activeChapter;
    }

    
    private IEnumerator ChapterFadeFlow(GameObject chapterGO)
    {
        yield return new WaitForSeconds(1.0f);

        // 1) FadeManager가 있으면 페이드인 실행
        if (FadeManager.Instance != null)
        {
            yield return FadeManager.Instance.FadeIn();
        }

        // 3) 화면에서 챕터 비활성화 (페이드 아웃 없이 바로 꺼짐)
        if (chapterGO != null)
        {
            chapterGO.SetActive(false);
        }

        // 4) 챕터 연출이 끝났을 때 콜백 호출
        if (OnChapterFinished != null)
        {
            OnChapterFinished();
        }
    }
}
