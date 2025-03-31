using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;               //UI를 컨트롤 할 것이라서 추가
using TMPro;                        // TextMeshPro를 사용하기 위해 필요
using UnityEngine;
using System;


public class StorySystem : MonoBehaviour
{
    public static StorySystem Instance;                 //간단한 싱글톤 화

    public StoryModel currentStoryModel;


    public float delay = 0.1f;                  // 각 글자가 나타나는 데 걸리는 시간
    public string fullText;                     // 전체 표시할 텍스트
    private string currentText = "";            // 현재까지 표시된 텍스트
    public TMP_Text textComponent;              // TextMeshPro 컴포넌트    
    public Image imageComponent;
    public Image imageComponent2;
    public Image imageComponent3;


    public Button[] buttonWay = new Button[3];
    public TMP_Text[] buttonWayText = new TMP_Text[3];



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < buttonWay.Length; i++)
        {
            int wayIndex = i; //클로저(closure) 문제를 해결
            // 클로저 문제란 람다식 또는 익명 함수가 외부 변수를 캡처할 때 발생하는 문제
            buttonWay[i].onClick.AddListener(() => OnWayClick(wayIndex));
        }
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
        StartCoroutine(ShowText());
    }

    public void ResetShow()
    {
        textComponent.text = "";

        for (int i = 0; i < buttonWay.Length; i++)
        {
            buttonWay[i].gameObject.SetActive(false);
        }
    }

    IEnumerator ShowText()
    {
        if (currentStoryModel.MainImage != null)
        {
            // Texture2D를 Sprite로 변환
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage.width, currentStoryModel.MainImage.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // 스프라이트의 축(중심) 지정
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage, rect, pivot);

            // Image 컴포넌트에 스프라이트 설정
            imageComponent.sprite = sprite;
        }
        if (currentStoryModel.MainImage2 != null)
        {
            // Texture2D를 Sprite로 변환
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage2.width, currentStoryModel.MainImage2.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // 스프라이트의 축(중심) 지정
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage2, rect, pivot);

            // Image 컴포넌트에 스프라이트 설정
            imageComponent2.sprite = sprite;
        }
        if (currentStoryModel.MainImage3 != null)
        {
            // Texture2D를 Sprite로 변환
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage3.width, currentStoryModel.MainImage3.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // 스프라이트의 축(중심) 지정
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage3, rect, pivot);

            // Image 컴포넌트에 스프라이트 설정
            imageComponent3.sprite = sprite;
        }
        else
        {
            Debug.LogError($"Unable to load texture: {currentStoryModel.MainImage.name}");
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

        yield return new WaitForSeconds(delay);
    }

    public void OnWayClick(int index)
    {
        StoryModel playStoryModel = currentStoryModel;
        Debug.Log($"[OnWayClick] 선택한 옵션 인덱스: {index}");

        // 선택된 옵션 가져오기
        StoryModel.Option selectedOption = playStoryModel.options[index];

        // 성공 결과가 있는 경우 실행
        if (selectedOption.eventCheck.sucessResult.Length > 0)
        {
            string buttonText = selectedOption.buttonText.Trim();

            if (buttonText != "넘어가기")
            {
                if (index == 0) GameSystem.Instance.choice1Count++;
                else if (index == 1) GameSystem.Instance.choice2Count++;
            }
            ProcessResult(selectedOption.eventCheck.sucessResult);
        }
    }

    // 결과를 처리하는 함수
    private void ProcessResult(StoryModel.Result[] results)
    {
        if (results.Length > 0)
        {
            Debug.Log($"[ProcessResult] 적용할 결과: {results[0].resultType}, 이동할 스토리 번호: {results[0].value}");
            GameSystem.Instance.ApplyChoice(results[0]); // 첫 번째 결과만 실행
        }
    }

}