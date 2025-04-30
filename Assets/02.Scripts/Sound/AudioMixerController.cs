using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;    
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private const string BGM_KEY = "BGMVolume";
    private const string SFX_KEY = "SFXVolume";

    //�����̴� MinValue�� 0.001

    private void Awake()
    {
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    private void Start()
    {
        float savedBGM = PlayerPrefs.GetFloat(BGM_KEY, 1f); // �⺻��: �ִ� ����
        float savedSFX = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        SetBGMVolume(savedBGM);
        SetSFXVolume(savedSFX);

        if (bgmSlider != null) bgmSlider.value = savedBGM;
        if (sfxSlider != null) sfxSlider.value = savedSFX;
    }

    public void SetBGMVolume(float value)
    {
        SetVolume("BGM", value);
        PlayerPrefs.SetFloat(BGM_KEY, value);
    }

    public void SetSFXVolume(float value)
    {
        SetVolume("SFX", value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    private void SetVolume(string parameterName, float value)
    {
        if (value <= 0.0001f)
        {
            // �����̴��� 0�� ������ ������ ���Ұ�(-80dB)
            audioMixer.SetFloat(parameterName, -80f);
        }
        else
        {
            // ���������� Log10 ��ȯ
            audioMixer.SetFloat(parameterName, Mathf.Log10(value) * 20f);
        }
    }
}