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

    public Option[] options; // ������ �迭


    [System.Serializable]
    public class Option
    {
        public string optionText;
        public string buttonText; // ������ ��ư�� �̸�

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

        public Result[] sucessResult; // �������� ���� ȿ�� �迭
        public Result[] failResult; // �������� ���� ȿ�� �迭
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
