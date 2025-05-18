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

    [Header("🎵 스토리 재생 시 필요한 사운드")]
    public AudioClip bgmClip;       // 스토리 구간에서 재생할 배경음악
    public AudioClip sfxClip;       // 스토리 텍스트 등장 시 효과음
    public AudioClip choiceSfxClip; // 선택지 클릭 시 효과음

    [System.Serializable]
    public class Option
    {
        public string buttonText;
        public EventCheck eventCheck;
        public List<int> requiredStoryNumbers;
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
