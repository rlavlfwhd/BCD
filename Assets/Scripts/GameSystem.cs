using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;


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
    public static GameSystem Instance;      //간단한 싱글톤 화

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
        if(slot < 1 || slot > 6)
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

        if(endingStoryIndex != -1)
        {
            currentStoryIndex = endingStoryIndex;
            StoryShow(currentStoryIndex);
        }        
    }


    public void StoryShow(int number)
    {
        StoryModel tempStoryModels = FindStoryModel(number);

        if(tempStoryModels != null)
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
        //if (storyNumber >= 11 && storyNumber < 21) return 1;
        //if(storyNumber >= 21 && storyNumber < 31) return 2;
        return -1;
    }

    private void ChangeChapter(int chapterIndex)
    {
        if(chapterIndex >= chapters.Length)
        {
            Debug.LogError("잘못된 챕터 인덱스!");
            return;
        }

        if (chapters[chapterIndex].activeSelf)
        {
            return;
        }


        activeChapter = chapters[chapterIndex];
        activeChapter.SetActive(true);
    }

    StoryModel FindStoryModel(int number)
    {
        foreach(var model in storyModels)
        {
            if(model.storyNumber == number)
            {
                return model;
            }
        }
        return null;
    }
}