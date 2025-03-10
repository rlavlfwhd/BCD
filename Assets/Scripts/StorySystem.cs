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

    public enum TEXTSYSTEM    
    {
        DOING,
        SELECT,
        DONE
    }

    public float delay = 0.1f;                  // �� ���ڰ� ��Ÿ���� �� �ɸ��� �ð�
    public string fullText;                     // ��ü ǥ���� �ؽ�Ʈ
    private string currentText = "";            // ������� ǥ�õ� �ؽ�Ʈ
    public TMP_Text textComponent;              // TextMeshPro ������Ʈ
    public TMP_Text storyIndex;                 // storyIndex 
    public Image imageComponent;                    

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

        storyIndex.text = currentStoryModel.storyNumber.ToString();

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

        if(currentStoryModel.voice != "")
        {
            SoundManager.instance.PlaySound(currentStoryModel.voice);
        }

        if (currentStoryModel.MainImage != null)
        {
            // Texture2D�� Sprite�� ��ȯ
            Rect rect = new Rect(0, 0, currentStoryModel.MainImage.width, currentStoryModel.MainImage.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f); // ��������Ʈ�� ��(�߽�) ����
            Sprite sprite = Sprite.Create(currentStoryModel.MainImage, rect, pivot);

            // Image ������Ʈ�� ��������Ʈ ����
            imageComponent.sprite = sprite;
        }
        else
        {
            Debug.LogError($"Unable to load texture: { currentStoryModel.MainImage.name}");
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
        bool CheckEventTypeNone = false;        //�⺻���� NONE�϶��� ������ �����̶� ���н� �ٽ� �Ҹ��°� ���ϱ�
        StoryModel playStoryModel = currentStoryModel;
        Debug.Log(index);

        if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.NONE)
        {
            for (int i = 0; i < playStoryModel.options[index].eventCheck.sucessResult.Length; i++)
            {
                GameSystem.Instance.ApplyChoice(currentStoryModel.options[index].eventCheck.sucessResult[i]);
                CheckEventTypeNone = true;
            }
        }

        bool CheckValue = false;

        if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.CheckSTR)
        {
            if (UnityEngine.Random.Range(0, GameSystem.Instance.stats.strength) >= playStoryModel.options[index].eventCheck.checkvalue)
            {
                CheckValue = true;
            }           
        }
        else if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.CheckDEX)
        {
            if (UnityEngine.Random.Range(0, GameSystem.Instance.stats.dexterity) >= playStoryModel.options[index].eventCheck.checkvalue)
            {
                CheckValue = true;
            }
        }
        else if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.CheckCON)
        {
            if (UnityEngine.Random.Range(0, GameSystem.Instance.stats.consitiution) >= playStoryModel.options[index].eventCheck.checkvalue)
            {
                CheckValue = true;
            }
        }
        else if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.CheckINT)
        {
            if (UnityEngine.Random.Range(0, GameSystem.Instance.stats.Intelligence) >= playStoryModel.options[index].eventCheck.checkvalue)
            {
                CheckValue = true;
            }
        }
        else if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.CheckCHA)
        {
            if (UnityEngine.Random.Range(0, GameSystem.Instance.stats.charisma) >= playStoryModel.options[index].eventCheck.checkvalue)
            {
                CheckValue = true;
            }
        }

        if (playStoryModel.options[index].eventCheck.eventType == StoryModel.EventCheck.EventType.CheckXP)
        {
            if (GameSystem.Instance.stats.currentXpPoint >= playStoryModel.options[index].eventCheck.checkvalue)
            {
                CheckValue = true;
            }

        }
            


        if (CheckValue)
        {
            for (int i = 0; i < playStoryModel.options[index].eventCheck.sucessResult.Length; i++)
            {
                GameSystem.Instance.ApplyChoice(playStoryModel.options[index].eventCheck.sucessResult[i]);
            }
        }
        else
        {
            if(CheckEventTypeNone == false)
            {
                for (int i = 0; i < playStoryModel.options[index].eventCheck.failResult.Length; i++)
                {
                    GameSystem.Instance.ApplyChoice(playStoryModel.options[index].eventCheck.failResult[i]);
                }
            }       
        }

    }

}
