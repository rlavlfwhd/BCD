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

    public const int A = 50;
    public const int B = 100;
    public const int C = 150;

    private void Awake()
    {
        Instance = this;
    }


    //=========================================================================================
    // 스토리
    //=========================================================================================

    public StoryModel[] storyModels;
    public int currentStoryIndex = 1;

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

    public void ApplyChoice(StoryModel.Result result)
    {
        switch (result.resultType)
        {            
            case StoryModel.Result.ResultType.GoToNextStory:
                currentStoryIndex += result.value;
                StoryShow(currentStoryIndex);
                break;

            case StoryModel.Result.ResultType.SelectA:
                currentStoryIndex += (result.value + A);
                StoryShow(currentStoryIndex);
                break;
                
            case StoryModel.Result.ResultType.SelectB:
                currentStoryIndex += (result.value + B);
                StoryShow(currentStoryIndex);
                break;

            case StoryModel.Result.ResultType.GoToEnding:
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

        if(tempStoryModels != null)
        {
            StorySystem.Instance.currentStoryModel = tempStoryModels;
            StorySystem.Instance.CoShowText();
        }
        else
        {
            Debug.LogError($"스토리 모델을 찾을 수 없음: {number}");
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