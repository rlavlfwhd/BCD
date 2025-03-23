using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel; // 옵션 UI 패널

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf); // 옵션 패널 표시/숨기기
    }
}
