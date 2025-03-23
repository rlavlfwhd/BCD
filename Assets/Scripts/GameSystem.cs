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

        // Reset Story Models ��ư ����
        if (GUILayout.Button("Reset Story Models"))
        {
            gameSystem.ResetStoryModels();
        }
    }
}
#endif

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;      //������ �̱��� ȭ

    private void Awake()
    {
        Instance = this;
    }

    public StoryModel[] storyModels;
    public int currentStoryIndex = 1;

    public int choice1Count = 0;
    public int choice2Count = 0;
    public int choice3Count = 0;

#if UNITY_EDITOR
    [ContextMenu("Reset Story Models")]
    public void ResetStoryModels()
    {
        storyModels = Resources.LoadAll<StoryModel>(""); // Resources ���� �Ʒ� ��� StoryModel �ҷ�����
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
        else
        {
            Debug.LogError("XXXXXXXXXXXXXX");
        }
    }


    public void StoryShow(int number)
    {
        StoryModel tempStoryModels = FindStoryModel(number);

        if(tempStoryModels != null)
        {
            StorySystem.Instance.currentStoryModel = tempStoryModels;
            StorySystem.Instance.CoShowText();
        }
        else
        {
            Debug.LogError($"���丮 ���� ã�� �� ����: {number}");
        }
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