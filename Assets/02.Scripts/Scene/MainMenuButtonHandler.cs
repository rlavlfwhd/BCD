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
        OptionManager.Instance.ShowConfirmPopup("������ �����Ͻðڽ��ϱ�?", OptionManager.Instance.QuitGameAction);
    }
}