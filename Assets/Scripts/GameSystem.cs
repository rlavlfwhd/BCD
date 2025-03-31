using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.Playables;  // PlayableDirector 사용을 위한 네임스페이스 추가

#if UNITY_EDITOR
[CustomEditor(typeof(GameSystem))]
public class GameSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameSystem gameSystem = (GameSystem)target;

        // Reset Story Models 버튼 생성
        if (GUILayout.Button("Reset Story Models"))
        {
            gameSystem.ResetStoryModels();
        }
    }
}
#endif

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    public GameObject[] chapters;
    public StoryModel[] storyModels;
    public int currentStoryIndex = 1;
    private GameObject activeChapter;

    public int choice1Count = 0;
    public int choice2Count = 0;
    public int choice3Count = 0;

#if UNITY_EDITOR
    [ContextMenu("Reset Story Models")]
    public void ResetStoryModels()
    {
        storyModels = Resources.LoadAll<StoryModel>(""); // Resources 폴더 아래 모든 StoryModel 불러오기
    }
#endif

    public void Start()
    {
        StoryShow(currentStoryIndex);
    }

    public void SaveGame(int slot)
    {
        if (slot < 1 || slot > 6)
        {
            return;
        }

        StoryModel currentStory = FindStoryModel(currentStoryIndex);
        string imagePath = "";

        if ((currentStory != null && currentStory.MainImage != null))
        {
            imagePath = AssetDatabase.GetAssetPath(currentStory.MainImage);
        }

        SaveSystem.SaveGame(slot, currentStoryIndex, choice1Count, choice2Count, choice3Count, imagePath);
    }

    public void LoadGame(int slot)
    {
        if (slot < 1 || slot > 6)
        {
            return;
        }

        SaveSystem.SaveData saveData = SaveSystem.LoadGame(slot);
        if (saveData != null)
        {
            currentStoryIndex = saveData.currentStoryIndex;
            choice1Count = saveData.choice1Count;
            choice2Count = saveData.choice2Count;
            choice3Count = saveData.choice3Count;
            StoryShow(currentStoryIndex);
        }
    }

    public void ApplyChoice(StoryModel.Result result)
    {
        switch (result.resultType)
        {
            case StoryModel.Result.ResultType.GoToNextStory:
                currentStoryIndex = result.value;
                StoryShow(currentStoryIndex);
                break;

            case StoryModel.Result.ResultType.GoToChoiceScene:
                SceneManager.LoadScene(result.changeSceneName);
                break;

            case StoryModel.Result.ResultType.GoToEnding:
                DetermineEnding();
                break;

            default:
                Debug.LogError("Unknown effect type");
                break;
        }
    }

    public void DetermineEnding()
    {
        int endingStoryIndex = -1;

        if (choice1Count > choice2Count)
        {
            endingStoryIndex = 500;
        }
        else if (choice1Count < choice2Count)
        {
            endingStoryIndex = 600;
        }

        if (endingStoryIndex != -1)
        {
            currentStoryIndex = endingStoryIndex;
            StoryShow(currentStoryIndex);
        }
    }


    public void StoryShow(int number)
    {
        StoryModel tempStoryModels = FindStoryModel(number);

        if (tempStoryModels != null)
        {
            StorySystem.Instance.currentStoryModel = tempStoryModels;
            StorySystem.Instance.CoShowText();

            int chapterIndex = GetChapterIndex(number);
            ChangeChapter(chapterIndex);
        }
        else
        {
            Debug.LogError($"스토리 모델을 찾을 수 없음: {number}");
        }
    }

    private int GetChapterIndex(int storyNumber)
    {
        if (storyNumber == 1) return 0;
        if (storyNumber >= 8 && storyNumber < 9) return 1;
        if (storyNumber >= 13 && storyNumber < 14) return 2;
        if (storyNumber >= 18 && storyNumber < 19) return 3;
        if (storyNumber >= 23 && storyNumber < 24) return 4;
        if (storyNumber >= 26 && storyNumber < 27) return 5;
        if (storyNumber >= 33 && storyNumber < 34) return 6;
        if (storyNumber >= 38 && storyNumber < 39) return 7;
        if (storyNumber >= 45 && storyNumber < 46) return 8;
        if (storyNumber >= 50 && storyNumber < 51) return 9;
        if (storyNumber >= 61 && storyNumber < 62) return 10;
        return -1;
    }

    private void ChangeChapter(int chapterIndex)
    {
        Debug.Log($"[ChangeChapter] 호출됨! 챕터 인덱스: {chapterIndex}");

        if (chapters == null || chapters.Length == 0)
        {
            Debug.LogError("챕터 배열이 비어 있습니다!");
            return;
        }

        if (chapterIndex < 0 || chapterIndex >= chapters.Length)
        {
            Debug.LogError($"잘못된 챕터 인덱스! index: {chapterIndex}");
            return;
        }

        // 현재 활성화된 챕터가 있다면 비활성화
        if (activeChapter != null)
        {
            Debug.Log($"[ChangeChapter] 이전 챕터 비활성화: {activeChapter.name}");
            PlayableDirector prevDirector = activeChapter.GetComponent<PlayableDirector>();
            if (prevDirector != null)
            {
                prevDirector.Stop(); // 타임라인 멈춤
            }
            activeChapter.SetActive(false); // 오브젝트 비활성화
        }

        // 새로운 챕터 활성화
        activeChapter = chapters[chapterIndex];
        activeChapter.SetActive(true);
        Debug.Log($"[ChangeChapter] 새로운 챕터 활성화: {activeChapter.name}");

        // PlayableDirector가 있는지 확인 후 실행
        PlayableDirector newDirector = activeChapter.GetComponent<PlayableDirector>();
        if (newDirector != null)
        {
            newDirector.playOnAwake = false; // 자동 실행 방지
            newDirector.Play();
            Debug.Log($"[ChangeChapter] PlayableDirector 실행: {newDirector.name}");

            // 모든 챕터에서 타임라인 끝나면 비활성화
            StartCoroutine(DisableChapterAfterTimeline(newDirector, activeChapter));
        }
    }



    // PlayableDirector 실행 완료 후 오브젝트 비활성화
    private IEnumerator DisableChapterAfterTimeline(PlayableDirector director, GameObject chapter)
    {
        yield return new WaitForSeconds((float)director.duration); // 타임라인 길이만큼 대기
        chapter.SetActive(false);
        Debug.Log($"[DisableChapterAfterTimeline] 챕터 비활성화됨: {chapter.name}");
    }

    // 일정 시간 후 챕터 비활성화 (PlayableDirector가 없는 경우)
    private IEnumerator DisableChapterAfterSeconds(GameObject chapter, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        chapter.SetActive(false);
        Debug.Log($"[DisableChapterAfterSeconds] 챕터 비활성화됨: {chapter.name}");
    }



    StoryModel FindStoryModel(int number)
    {
        foreach (var model in storyModels)
        {
            if (model.storyNumber == number)
            {
                return model;
            }
        }
        return null;
    }
}
