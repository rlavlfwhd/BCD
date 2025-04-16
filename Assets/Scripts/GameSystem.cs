using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using UnityEngine.Playables;  // PlayableDirector 사용을 위한 네임스페이스 추가

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

    public CameraParallax cameraParallax;
    public GameObject[] chapters;
    public StoryModel[] storyModels;
    public int currentStoryIndex = 1;
    public GameObject activeChapter;
    private HashSet<string> completedPuzzleFlags = new HashSet<string>();

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

    public void Start()
    {
        if (chapters == null || chapters.Length == 0)
        {
            chapters = GameObject.FindGameObjectsWithTag("Chapter");
            Debug.Log(" chapters 재할당됨: " + chapters.Length);
        }

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
        string imagePath2 = "";


        if (currentStory != null)
        {
            if (currentStory.MainImage != null)
            {
                imagePath = Application.persistentDataPath + $"/slot{slot}_image1.png";
                File.WriteAllBytes(imagePath, currentStory.MainImage.EncodeToPNG());
            }

            if (currentStory.MainImage2 != null)
            {
                imagePath2 = Application.persistentDataPath + $"/slot{slot}_image2.png";
                File.WriteAllBytes(imagePath2, currentStory.MainImage2.EncodeToPNG());
            }
        }

        List<string> itemNames = new List<string>();
        foreach (var item in Inventory.Instance.items)
        {
            itemNames.Add(item.itemName);
        }

        SaveSystem.SaveGame(slot, currentStory.storyNumber, imagePath, imagePath2, itemNames, PuzzleManager.Instance.GetCompletedPuzzleList());
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
            StoryShow(currentStoryIndex);
        }

        if (saveData.inventoryItemNames != null)
        {
            Inventory.Instance.items.Clear();
            foreach (string itemName in saveData.inventoryItemNames)
            {
                Item loadedItem = Resources.Load<Item>("Items/" + itemName); // 경로 주의
                if (loadedItem != null)
                {
                    Inventory.Instance.items.Add(loadedItem);
                }
                else
                {
                    Debug.LogWarning("해당 아이템 로드 실패: " + itemName);
                }
            }
            Inventory.Instance.FreshSlot(); // 슬롯 UI 갱신

            if (saveData.completedPuzzles != null)
            {
                PuzzleManager.Instance.SetCompletedPuzzleList(saveData.completedPuzzles);
            }
        }

        PuzzleManager.Instance.SetCompletedPuzzleList(saveData.completedPuzzles);
    }
    public void MarkPuzzleComplete(string puzzleName)
    {
        completedPuzzleFlags.Add(puzzleName);
    }
    public bool IsPuzzleCompleted(string puzzleName)
    {
        return completedPuzzleFlags.Contains(puzzleName);
    }
    public List<string> GetCompletedPuzzleList()
    {
        return new List<string>(completedPuzzleFlags);
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

            default:
                Debug.LogError("Unknown effect type");
                break;
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

            if (chapterIndex == -1 || chapters[chapterIndex].GetComponent<PlayableDirector>() == null)
            {
                StartCoroutine(StorySystem.Instance.ShowText());
            }
        }
        else
        {
            Debug.LogError($"스토리 모델을 찾을 수 없음: {number}");
        }

        if (number >= 30) // 챕터 2 시작이면
        {
            Inventory.Instance.items.Clear();
            Inventory.Instance.FreshSlot();
            Debug.Log("챕터 2 시작 - 인벤토리 초기화");
        }
    }

    public int GetChapterIndex(int storyNumber)
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

    public void ChangeChapter(int chapterIndex)
    {
        if (chapterIndex == -1)
        {
            return;
        }

        if (chapters == null || chapters.Length == 0)
        {
            return;
        }

        if (chapterIndex < 0 || chapterIndex >= chapters.Length)
        {
            return;
        }

        if (activeChapter != null)
        {
            PlayableDirector prevDirector = activeChapter.GetComponent<PlayableDirector>();                        
            if (prevDirector != null)
            {
                prevDirector.Stop();                
            }
            activeChapter.SetActive(false);
        }

        activeChapter = chapters[chapterIndex];
        activeChapter.SetActive(true);

        PlayableDirector newDirector = activeChapter.GetComponent<PlayableDirector>();
        if (newDirector != null)
        {
            newDirector.playOnAwake = false;
            newDirector.Play();
            cameraParallax.ResetToInitialPosition();
            cameraParallax.enabled = false;
            StartCoroutine(DisableChapterAfterTimeline(newDirector, activeChapter));            
        }
        else
        {
            // 타임라인이 없으면 일정 시간 후 비활성화 + 스토리 텍스트 출력
            StartCoroutine(DisableChapterAfterSeconds(activeChapter, 2.0f)); // 2초 후 실행
        }
    }




    // PlayableDirector 실행 완료 후 오브젝트 비활성화
    private IEnumerator DisableChapterAfterTimeline(PlayableDirector director, GameObject chapter)
    {
        yield return new WaitForSeconds((float)director.duration); // 타임라인 길이만큼 대기
        chapter.SetActive(false);
        cameraParallax.enabled = true;

        Debug.Log($"[DisableChapterAfterTimeline] 챕터 비활성화됨: {chapter.name}");

        if (StorySystem.Instance.currentStoryModel != null)
        {
            StartCoroutine(StorySystem.Instance.ShowText());
        }
    }

    // 일정 시간 후 챕터 비활성화 (PlayableDirector가 없는 경우)
    private IEnumerator DisableChapterAfterSeconds(GameObject chapter, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        chapter.SetActive(false);
        Debug.Log($"[DisableChapterAfterSeconds] 챕터 비활성화됨: {chapter.name}");

        if (StorySystem.Instance.currentStoryModel != null)
        {
            StartCoroutine(StorySystem.Instance.ShowText());
        }
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
