using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StorySystem : MonoBehaviour
{
    public static StorySystem Instance;

    public StoryModel[] storyModels;
    public int currentStoryIndex = 1;
    public StoryModel currentStoryModel;
    private int lastPlayedChapterIndex = -1;

    public float delay = 0.01f;
    public string fullText;
    private string currentText = "";

    public TMP_Text textComponent;
    public Image imageComponent;
    public Image imageComponent2;

    public Button[] buttonWay = new Button[3];
    public TMP_Text[] buttonWayText = new TMP_Text[3];

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        Instance = this;

        for (int i = 0; i < buttonWay.Length; i++)
        {
            int wayIndex = i;
            buttonWay[i].onClick.AddListener(() => OnWayClick(wayIndex));
        }

        yield return new WaitUntil(() => SceneDataManager.Instance != null && SceneDataManager.Instance.Data != null);

        int overrideStoryIndex = SceneDataManager.Instance.Data.nextStoryIndex;
        if (overrideStoryIndex > 0)
        {
            currentStoryIndex = overrideStoryIndex;
            SceneDataManager.Instance.Data.nextStoryIndex = 0;
        }

        StoryShow(currentStoryIndex);
    }

    public void StoryModelInit()
    {
        fullText = currentStoryModel.storyText;

        for (int i = 0; i < buttonWay.Length; i++)
        {
            bool show = i < currentStoryModel.options.Length;

            if (show)
            {
                var option = currentStoryModel.options[i];
                show = IsOptionVisible(option);

                if (show)
                    buttonWayText[i].text = option.buttonText;
            }

            buttonWay[i].gameObject.SetActive(show);
        }
    }

    private bool IsOptionVisible(StoryModel.Option option)
    {
        bool hasCondition = false;

        if (option.requiredStoryNumbers != null && option.requiredStoryNumbers.Count > 0)
        {
            hasCondition = true;
            foreach (int required in option.requiredStoryNumbers)
            {
                if (!SceneDataManager.Instance.Data.seenStoryNumbers.Contains(required))
                    return false;
            }
        }

        if (option.requiredCompletedPuzzles != null && option.requiredCompletedPuzzles.Count > 0)
        {
            hasCondition = true;
            foreach (string puzzleID in option.requiredCompletedPuzzles)
            {
                if (!SceneDataManager.Instance.Data.completedPuzzles.Contains(puzzleID))
                    return false;
            }
        }

        // 조건이 없으면 그냥 true
        if (!hasCondition)
            return true;

        return true;
    }



    public void CoShowText()
    {
        StoryModelInit();
        ResetShow();
        StartCoroutine(ShowImage());
    }

    public void ResetShow()
    {
        textComponent.text = "";
        for (int i = 0; i < buttonWay.Length; i++)
        {
            buttonWay[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator ShowText()
    {
        if (string.IsNullOrEmpty(fullText))
        {
            Debug.LogError("[ShowText] fullText가 비어있음.");
            yield break;
        }

        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            textComponent.text = currentText;
            yield return new WaitForSeconds(delay);
        }

        for (int i = 0; i < currentStoryModel.options.Length && i < buttonWay.Length; i++)
        {
            var option = currentStoryModel.options[i];
            if (IsOptionVisible(option))
            {
                buttonWay[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    IEnumerator ShowImage()
    {
        if (currentStoryModel.MainImage != null)
        {
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage.width, currentStoryModel.MainImage.height);
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage, rect, new Vector2(0.5f, 0.5f));
            imageComponent.sprite = sprite;
        }

        if (currentStoryModel.MainImage2 != null)
        {
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage2.width, currentStoryModel.MainImage2.height);
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage2, rect, new Vector2(0.5f, 0.5f));
            imageComponent2.sprite = sprite;
        }

        yield return new WaitForSeconds(delay);
    }

    public void OnWayClick(int index)
    {
        if (currentStoryModel.choiceSfxClip != null)
        {
            SoundManager.instance.PlaySFX(currentStoryModel.choiceSfxClip);
        }

        StoryModel.Option selectedOption = currentStoryModel.options[index];
        if (selectedOption.eventCheck.sucessResult.Length > 0)
        {
            ProcessResult(selectedOption.eventCheck.sucessResult);
        }
    }

    private void ProcessResult(StoryModel.Result[] results)
    {
        if (results.Length > 0)
        {
            ApplyChoice(results[0]);
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

            default:
                Debug.LogError("Unknown effect type");
                break;
        }
    }

    public void StoryShow(int number)
    {
        if (!SceneDataManager.Instance.Data.seenStoryNumbers.Contains(number))
            SceneDataManager.Instance.Data.seenStoryNumbers.Add(number);

        currentStoryModel = FindStoryModel(number);

        if (currentStoryModel != null)
        {
            if (currentStoryModel.bgmClip != null)
                SoundManager.instance.PlayBGM(currentStoryModel.bgmClip);

            if (currentStoryModel.sfxClip != null)
                SoundManager.instance.PlaySFX(currentStoryModel.sfxClip);

            CoShowText();

            int chapterIndex = ChapterController.GetChapterIndexForStoryNumber(number);

            if (chapterIndex != lastPlayedChapterIndex)
            {
                ChapterController.Instance.OnChapterFinished = () =>
                {
                    StartCoroutine(ShowText());
                    ChapterController.Instance.OnChapterFinished = null;
                };

                ChapterController.Instance.ChangeChapter(chapterIndex);
                lastPlayedChapterIndex = chapterIndex;
            }
            else
            {
                StartCoroutine(ShowText());
            }
        }
        else
        {
            Debug.LogError($"스토리 모델을 찾을 수 없음: {number}");
        }
    }



    StoryModel FindStoryModel(int number)
    {
        foreach (var model in storyModels)
        {
            if (model.storyNumber == number)
                return model;
        }
        return null;
    }
}
