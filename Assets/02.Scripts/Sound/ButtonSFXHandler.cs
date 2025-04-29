using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFXHandler : MonoBehaviour
{
    [Header("커스텀 버튼 클릭 사운드 (없으면 기본 사용)")]
    public AudioClip customSFX;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayButtonSFX);
    }

    private void PlayButtonSFX()
    {
        if(SoundManager.instance == null)
        {
            return;
        }

        if(customSFX != null)
        {
            SoundManager.instance.PlaySFX(customSFX);
        }
        else
        {
            SoundManager.instance.PlaySFX(SoundManager.instance.defaultButtonClickSFX);
        }
    }
}
