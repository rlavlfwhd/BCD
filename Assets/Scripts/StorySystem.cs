using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System;

public class StorySystem : MonoBehaviour
{
    public static StorySystem Instance;

    public StoryModel currentStoryModel;

    public float delay = 0.1f;
    public string fullText;
    public string narrationFullText;  // ✅ 나레이션 텍스트
    private string currentText = "";

    public TMP_Text textComponent;              // 캐릭터 대사 텍스트
    public TMP_Text narrationTextComponent;     // 나레이션 텍스트

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

        if (GameSystem.Instance != null)
        {
            GameSystem.Instance.StoryShow(GameSystem.Instance.currentStoryIndex);
        }
    }

    public void StoryModelInit()
    {
        fullText = currentStoryModel.storyText;
        narrationFullText = currentStoryModel.narrationText;  // ✅ 나레이션 텍스트도 초기화

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

    public IEnumerator CoShowNarrationText()
    {
        StoryModelInit();
        ResetShow();
        yield return StartCoroutine(ShowText());
    }

    public void ResetShow()
    {
        textComponent.text = "";
        if (narrationTextComponent != null)
            narrationTextComponent.text = "";  // ✅ 나레이션 텍스트도 초기화

        for (int i = 0; i < buttonWay.Length; i++)
        {
            buttonWay[i].gameObject.SetActive(false);
        }
    }

    public IEnumerator ShowText()
    {
        // 캐릭터 대사 출력
        if (!string.IsNullOrEmpty(fullText))
        {
            for (int i = 0; i <= fullText.Length; i++)
            {
                currentText = fullText.Substring(0, i);
                textComponent.text = currentText;
                yield return new WaitForSeconds(delay);
            }
        }

        // 나레이션 텍스트 출력
        if (!string.IsNullOrEmpty(narrationFullText) && narrationTextComponent != null)
        {
            for (int i = 0; i <= narrationFullText.Length; i++)
            {
                narrationTextComponent.text = narrationFullText.Substring(0, i);
                yield return new WaitForSeconds(delay);
            }
        }

        // 선택지 출력
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
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage, rect, pivot);
            imageComponent.sprite = sprite;
        }

        if (currentStoryModel.MainImage2 != null)
        {
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage2.width, currentStoryModel.MainImage2.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage2, rect, pivot);
            imageComponent2.sprite = sprite;
        }
        else
        {
            Debug.LogError($"Unable to load texture: {currentStoryModel.MainImage.name}");
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(ShowText());
    }

    public void OnWayClick(int index)
    {
        StoryModel playStoryModel = currentStoryModel;
        Debug.Log($"[OnWayClick] 선택한 옵션 인덱스: {index}");

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
            Debug.Log($"[ProcessResult] 적용할 결과: {results[0].resultType}, 이동할 스토리 번호: {results[0].value}");
            GameSystem.Instance.ApplyChoice(results[0]);
        }
    }
}