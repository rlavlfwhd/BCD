using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtonHandler : MonoBehaviour
{
    public void OpenLoadPanel()
    {
        OptionManager.Instance.LoadPanelBtnClick();
    }

    public void OpenSoundSettingPanel()
    {
        OptionManager.Instance.SoundSettingPanelBtnClick();
    }

    public void QuitGamePopup()
    {
        OptionManager.Instance.ShowConfirmPopup("게임을 종료하시겠습니까?", OptionManager.Instance.QuitGameAction);
    }
}