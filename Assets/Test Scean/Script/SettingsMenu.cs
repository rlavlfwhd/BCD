using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel; // �ɼ� UI �г�

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf); // �ɼ� �г� ǥ��/�����
    }
}
