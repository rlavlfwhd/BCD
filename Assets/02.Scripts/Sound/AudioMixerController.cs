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
        InitSlider(bgmSlider, "BGM");
        InitSlider(sfxSlider, "SFX");
    }

    private void InitSlider(Slider slider, string parameterName)
    {
        if (slider == null)
            return;

        float currentVolume;
        if (audioMixer.GetFloat(parameterName, out currentVolume))
        {
            slider.value = Mathf.Pow(10f, currentVolume / 20f);
        }
    }

    public void SetBGMVolume(float value)
    {
        SetVolume("BGM", value);
    }

    public void SetSFXVolume(float value)
    {
        SetVolume("SFX", value);
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