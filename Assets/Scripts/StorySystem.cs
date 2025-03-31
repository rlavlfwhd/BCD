using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;               //UI�� ��Ʈ�� �� ���̶� �߰�
using TMPro;                        // TextMeshPro�� ����ϱ� ���� �ʿ�
using UnityEngine;
using System;


public class StorySystem : MonoBehaviour
{
    public static StorySystem Instance;                 //������ �̱��� ȭ

    public StoryModel currentStoryModel;


    public float delay = 0.1f;                  // �� ���ڰ� ��Ÿ���� �� �ɸ��� �ð�
    public string fullText;                     // ��ü ǥ���� �ؽ�Ʈ
    private string currentText = "";            // ������� ǥ�õ� �ؽ�Ʈ
    public TMP_Text textComponent;              // TextMeshPro ������Ʈ    
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
            int wayIndex = i; //Ŭ����(closure) ������ �ذ�
            // Ŭ���� ������ ���ٽ� �Ǵ� �͸� �Լ��� �ܺ� ������ ĸó�� �� �߻��ϴ� ����
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
            // Texture2D�� Sprite�� ��ȯ
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage.width, currentStoryModel.MainImage.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // ��������Ʈ�� ��(�߽�) ����
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage, rect, pivot);

            // Image ������Ʈ�� ��������Ʈ ����
            imageComponent.sprite = sprite;
        }
        if (currentStoryModel.MainImage2 != null)
        {
            // Texture2D�� Sprite�� ��ȯ
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage2.width, currentStoryModel.MainImage2.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // ��������Ʈ�� ��(�߽�) ����
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage2, rect, pivot);

            // Image ������Ʈ�� ��������Ʈ ����
            imageComponent2.sprite = sprite;
        }
        if (currentStoryModel.MainImage3 != null)
        {
            // Texture2D�� Sprite�� ��ȯ
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage3.width, currentStoryModel.MainImage3.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // ��������Ʈ�� ��(�߽�) ����
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage3, rect, pivot);

            // Image ������Ʈ�� ��������Ʈ ����
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
        Debug.Log($"[OnWayClick] ������ �ɼ� �ε���: {index}");

        // ���õ� �ɼ� ��������
        StoryModel.Option selectedOption = playStoryModel.options[index];

        // ���� ����� �ִ� ��� ����
        if (selectedOption.eventCheck.sucessResult.Length > 0)
        {
            string buttonText = selectedOption.buttonText.Trim();

            if (buttonText != "�Ѿ��")
            {
                if (index == 0) GameSystem.Instance.choice1Count++;
                else if (index == 1) GameSystem.Instance.choice2Count++;
            }
            ProcessResult(selectedOption.eventCheck.sucessResult);
        }
    }

    // ����� ó���ϴ� �Լ�
    private void ProcessResult(StoryModel.Result[] results)
    {
        if (results.Length > 0)
        {
            Debug.Log($"[ProcessResult] ������ ���: {results[0].resultType}, �̵��� ���丮 ��ȣ: {results[0].value}");
            GameSystem.Instance.ApplyChoice(results[0]); // ù ��° ����� ����
        }
    }

}