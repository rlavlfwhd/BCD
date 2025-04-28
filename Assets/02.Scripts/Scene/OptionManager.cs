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
    public static OptionManager Instance;

    public TMP_Text currentOptionText;
    public GameObject optionPanel;
    public GameObject savePanel;
    public GameObject loadPanel;
    public GameObject soundSettingPanel;
    public GameObject saveTextBtn;
    public GameObject mainSceneTextBtn;
    public GameObject[] inGameBtns;

    public GameObject confirmPopup;
    public TMP_Text confirmText;
    public CanvasGroup confirmPopupCanvasGroup;
    private System.Action confirmYesAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // �� �ε�� ������ �̺�Ʈ ����
        SceneManager.sceneLoaded += OnSceneChanged;

        // ���� �������� �ٷ� üũ
        CheckSceneButtons(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        // �̺�Ʈ �ߺ� ����
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        CheckSceneButtons(scene.name);
    }

    private void CheckSceneButtons(string sceneName)
    {
        foreach(var btns in inGameBtns)
        {
            if (sceneName == "MainScene")
            {
                saveTextBtn.SetActive(false);
                mainSceneTextBtn.SetActive(false);
                btns.gameObject.SetActive(false);

            }
            else
            {
                saveTextBtn.SetActive(true);
                mainSceneTextBtn.SetActive(true);
                btns.gameObject.SetActive(true);
            }
        }
    }


    private void DisableAllPanels()
    {
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
        soundSettingPanel.SetActive(false);
    }

    public void SavePanelBtnClick()
    {
        DisableAllPanels();
        if (!optionPanel.activeSelf)
        {
            optionPanel.SetActive(true);
        }
        savePanel.SetActive(true);
        currentOptionText.text = "����";
    }

    public void LoadPanelBtnClick()
    {
        DisableAllPanels();
        if (!optionPanel.activeSelf)
        {
            optionPanel.SetActive(true);
        }
        loadPanel.SetActive(true);
        currentOptionText.text = "�ҷ�����";
    }

    public void SoundSettingPanelBtnClick()
    {
        DisableAllPanels();
        if(!optionPanel.activeSelf)
        {
            optionPanel.SetActive(true);
        }
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

    public void ShowConfirmPopup(string message, System.Action yesAction)
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
            if (optionPanel != null)
            {
                optionPanel.SetActive(false);
            }
        }

        // �̺�Ʈ �ߺ� ����: �� �� �������� ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �˾� �� �� ���� ����
    public void QuitGameAction()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
