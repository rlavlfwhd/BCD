using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using DG.Tweening;

public class OptionManager : MonoBehaviour
{
    public TMP_Text currentOptionText;
    public GameObject optionPanel;
    public GameObject savePanel;
    public GameObject loadPanel;
    public GameObject soundSettingPanel;

    public GameObject confirmPopup;
    public TMP_Text confirmText;
    public CanvasGroup confirmPopupCanvasGroup;
    private System.Action confirmYesAction;

    private void DisableAllPanels()
    {
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        soundSettingPanel.SetActive(false);
    }

    public void SavePanelBtnClick()
    {
        DisableAllPanels();
        savePanel.SetActive(true);
        currentOptionText.text = "����";
    }

    public void LoadPanelBtnClick()
    {
        DisableAllPanels();
        loadPanel.SetActive(true);
        currentOptionText.text = "�ҷ�����";
    }

    public void SoundSettingPanelBtnClick()
    {
        DisableAllPanels();
        soundSettingPanel.SetActive(true);
        currentOptionText.text = "�Ҹ� ����";
    }

    public void MainMenuBtnClick()
    {
        ShowConfirmPopup("���� �޴��� ���ư��ðڽ��ϱ�?", MainMenuAction);
    }

    public void QuitBtnClick()
    {
        ShowConfirmPopup("������ �����Ͻðڽ��ϱ�?", QuitGameAction);
    }

    public void ReturnBtnClick()
    {
        optionPanel.SetActive(false);
    }

    private void ShowConfirmPopup(string message, System.Action yesAction)
    {
        confirmText.text = message;
        confirmYesAction = yesAction;

        confirmPopup.SetActive(true);
        confirmPopupCanvasGroup.alpha = 0f;
        confirmPopupCanvasGroup.DOFade(1f, 0.3f);  // ���̵� ��
    }

    public void OnConfirmYes()
    {
        CloseConfirmPopup(() =>
        {
            if (confirmYesAction != null)
            {
                confirmYesAction();                
            }
        });
    }

    // �˾�: �ƴϿ� ��ư
    public void OnConfirmNo()
    {
        CloseConfirmPopup();
    }

    private void CloseConfirmPopup(Action onComplete = null)
    {
        confirmPopupCanvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
        {
            confirmPopup.SetActive(false);
            if (onComplete != null)
            {
                onComplete();
            }
        });
    }

    // �˾� �� �� ���� �޴� �̵�
    private void MainMenuAction()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            GameObject optionPanel = GameObject.Find("OptionPanel");  // MainScene�� �ִ� ������Ʈ ã�Ƽ�
            if (optionPanel != null)
            {
                optionPanel.SetActive(false);
            }
        }

        // �̺�Ʈ �ߺ� ����: �� �� �������� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �˾� �� �� ���� ����
    private void QuitGameAction()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
