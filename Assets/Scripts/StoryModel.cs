using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStory", menuName = "ScriptableObjects/StoryModel")]
public class StoryModel : ScriptableObject
{
    public int storyNumber;
    public int storyLevelMin;
    public int storyLevelMax;

    public Texture2D MainImage;

    public string voice;
    
    public enum STORYTYPE
    {
        MAIN,
        SUB,
        SERIAL
    }

    public STORYTYPE storytype;

    public bool storyDone;

    [TextArea(10, 10)]
    public string storyText;

    public Option[] options; // 선택지 배열


    [System.Serializable]
    public class Option
    {
        public string optionText;
        public string buttonText; // 선택지 버튼의 이름

        public EventCheck eventCheck;
    }

    [System.Serializable]
    public class EventCheck
    {
        public int checkvalue;
        public enum EventType : int
        {
            NONE,
            GoToBattle,
            CheckSTR,
            CheckDEX,
            CheckCON,
            CheckINT,
            CheckWIS,
            CheckCHA,
            CheckXP,

        }

        public EventType eventType;

        public Result[] sucessResult; // 선택지에 대한 효과 배열
        public Result[] failResult; // 선택지에 대한 효과 배열
    }

    [System.Serializable]
    public class Result
    {
        public enum ResultType : int
        {
            ChangeHp,
            ChangeSp,
            AddExperience,
            GoToShop,
            GoToNextStory,
            GoToRandomStory,
            GoToEnding 
        }

        public ResultType resultType;
        public int value;
        public string changeSceneName;
        public Stats stats;
    }

}
