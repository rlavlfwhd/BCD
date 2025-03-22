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

    public Option[] options; // ������ �迭



    [System.Serializable]
    public class Option
    {        
        public string buttonText; // ������ ��ư�� �̸�

        public EventCheck eventCheck;
    }

    [System.Serializable]
    public class EventCheck
    {
        public Result[] sucessResult; // �������� ���� ȿ�� �迭        
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