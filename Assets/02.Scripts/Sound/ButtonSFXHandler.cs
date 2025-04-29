using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFXHandler : MonoBehaviour
{
    [Header("Ŀ���� ��ư Ŭ�� ���� (������ �⺻ ���)")]
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
