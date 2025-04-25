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

    public CameraParallax cameraParallax;
    public GameObject[] chapters;
    public GameObject activeChapter;

    public StoryModel[] storyModels;
    public int currentStoryIndex = 1;
    public StoryModel currentStoryModel;

    public float delay = 0.1f;
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

    private void Start()
    {
        Instance = this;

        for (int i = 0; i < buttonWay.Length; i++)
        {
            int wayIndex = i;
            buttonWay[i].onClick.AddListener(() => OnWayClick(wayIndex));
        }

        if (chapters == null || chapters.Length == 0)
        {
            chapters = GameObject.FindGameObjectsWithTag("Chapter");
        }

        // 새로 추가된 부분 
        int overrideStoryIndex = SceneDataManager.Instance.Data.nextStoryIndex;
        if (overrideStoryIndex > 0)
        {
            currentStoryIndex = overrideStoryIndex;
            SceneDataManager.Instance.Data.nextStoryIndex = 0; // 초기화
        }

        StoryShow(currentStoryIndex);
    }

    public void StoryModelInit()
    {
        fullText = currentStoryModel.storyText;

        for (int i = 0; i < currentStoryModel.options.Length; i++)
        {
            buttonWayText[i].text = currentStoryModel.options[i].buttonText;
        }
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

        for (int i = 0; i < currentStoryModel.options.Length; i++)
        {
            buttonWay[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
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
        StoryModel playStoryModel = currentStoryModel;
        StoryModel.Option selectedOption = playStoryModel.options[index];

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
        currentStoryModel = FindStoryModel(number);

        if (currentStoryModel != null)
        {
            CoShowText();
            int chapterIndex = GetChapterIndex(number);
            ChangeChapter(chapterIndex);

            if (chapterIndex == -1 || chapters[chapterIndex].GetComponent<PlayableDirector>() == null)
            {
                StartCoroutine(ShowText());
            }
        }
        else
        {
            Debug.LogError($"스토리 모델을 찾을 수 없음: {number}");
        }
    }

    int GetChapterIndex(int storyNumber)
    {
        if (storyNumber == 1) return 0;
        if (storyNumber >= 8 && storyNumber < 9) return 1;
        if (storyNumber >= 13 && storyNumber < 14) return 2;
        if (storyNumber >= 18 && storyNumber < 19) return 3;
        if (storyNumber >= 23 && storyNumber < 24) return 4;
        if (storyNumber >= 26 && storyNumber < 27) return 5;
        if (storyNumber >= 33 && storyNumber < 34) return 6;
        if (storyNumber >= 38 && storyNumber < 39) return 7;
        if (storyNumber >= 45 && storyNumber < 46) return 8;
        if (storyNumber >= 50 && storyNumber < 51) return 9;
        if (storyNumber >= 61 && storyNumber < 62) return 10;
        return -1;
    }

    void ChangeChapter(int chapterIndex)
    {
        if (chapterIndex == -1 || chapters == null || chapters.Length == 0 || chapterIndex >= chapters.Length)
        {
            return;
        }

        if (activeChapter != null)
        {
            PlayableDirector prevDirector = activeChapter.GetComponent<PlayableDirector>();
            if (prevDirector != null)
            {
                prevDirector.Stop();
            }
            activeChapter.SetActive(false);
        }

        activeChapter = chapters[chapterIndex];
        activeChapter.SetActive(true);

        PlayableDirector newDirector = activeChapter.GetComponent<PlayableDirector>();
        if (newDirector != null)
        {
            newDirector.playOnAwake = false;
            newDirector.Play();
            cameraParallax.ResetToInitialPosition();
            cameraParallax.enabled = false;
            StartCoroutine(DisableChapterAfterTimeline(newDirector, activeChapter));
        }
        else
        {
            StartCoroutine(DisableChapterAfterSeconds(activeChapter, 2.0f));
        }
    }

    private IEnumerator DisableChapterAfterTimeline(PlayableDirector director, GameObject chapter)
    {
        yield return new WaitForSeconds((float)director.duration);
        chapter.SetActive(false);
        cameraParallax.enabled = true;

        if (currentStoryModel != null)
        {
            StartCoroutine(ShowText());
        }
    }

    private IEnumerator DisableChapterAfterSeconds(GameObject chapter, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        chapter.SetActive(false);

        if (currentStoryModel != null)
        {
            StartCoroutine(ShowText());
        }
    }

    StoryModel FindStoryModel(int number)
    {
        foreach (var model in storyModels)
        {
            if (model.storyNumber == number)
            {
                return model;
            }
        }
        return null;
    }
}
