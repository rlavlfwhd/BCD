using System.Collections;
using System.Collections.Generic;
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
    public string storyText; // ✅ 캐릭터 대사 텍스트

    [TextArea(10, 10)]
    public string narrationText; // ✅ 나레이션 텍스트 (새로 추가된 필드)

    public bool isNarration; // ✅ 나레이션 여부 (기존대로 유지)

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
