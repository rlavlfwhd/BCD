using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStory", menuName = "ScriptableObjects/StoryModel")]
public class StoryModel : ScriptableObject
{   
    public int storyNumber;
    public Texture2D MainImage;    
    public Texture2D MainImage2;



    public enum StoryType
    {
        MAIN,
        TypeA,
        TypeB,
        TypeC
    }

    public StoryType storyType;

    [TextArea(10, 10)]
    public string storyText;

    public Option[] options; // 선택지 배열



    [System.Serializable]
    public class Option
    {        
        public string buttonText; // 선택지 버튼의 이름

        public EventCheck eventCheck;
    }

    [System.Serializable]
    public class EventCheck
    {
        public Result[] sucessResult; // 선택지에 대한 효과 배열        
    }

    [System.Serializable]
    public class Result
    {
        public enum ResultType : int
        {
            GoToNextStory,            
            GoToEnding,
            GoToChoiceScene
        }

        public ResultType resultType;
        public int value;
        public string changeSceneName;        
    }

}