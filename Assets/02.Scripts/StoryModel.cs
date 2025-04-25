using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStory", menuName = "ScriptableObjects/StoryModel")]
public class StoryModel : ScriptableObject
{
    public int storyNumber;
    public Texture2D MainImage;
    public Texture2D MainImage2;


    [TextArea(10, 10)]
    public string storyText;

    public Option[] options;

    [System.Serializable]
    public class Option
    {
        public string buttonText;
        public EventCheck eventCheck;
    }

    [System.Serializable]
    public class EventCheck
    {
        public Result[] sucessResult;
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
