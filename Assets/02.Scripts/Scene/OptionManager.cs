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
        currentOptionText.text = "저장";
    }

    public void LoadPanelBtnClick()
    {
        DisableAllPanels();
        loadPanel.SetActive(true);
        currentOptionText.text = "불러오기";
    }

    public void SoundSettingPanelBtnClick()
    {
        DisableAllPanels();
        soundSettingPanel.SetActive(true);
        currentOptionText.text = "소리 설정";
    }

    public void MainMenuBtnClick()
    {
        ShowConfirmPopup("메인 메뉴로 돌아가시겠습니까?", MainMenuAction);
    }

    public void QuitBtnClick()
    {
        ShowConfirmPopup("게임을 종료하시겠습니까?", QuitGameAction);
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
        confirmPopupCanvasGroup.DOFade(1f, 0.3f);  // 페이드 인
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

    // 팝업: 아니오 버튼
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

    // 팝업 예 → 메인 메뉴 이동
    private void MainMenuAction()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("MainScene");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            GameObject optionPanel = GameObject.Find("OptionPanel");  // MainScene에 있는 오브젝트 찾아서
            if (optionPanel != null)
            {
                optionPanel.SetActive(false);
            }
        }

        // 이벤트 중복 방지: 한 번 끝났으면 제거
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 팝업 예 → 게임 종료
    private void QuitGameAction()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
