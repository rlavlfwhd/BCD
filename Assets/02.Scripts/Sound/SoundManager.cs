using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Default SFX")]
    public AudioClip defaultButtonClickSFX;

    [Header("BGM 및 SFX 관리자")]
    public BGMManager bgmManager;
    public SFXManager sfxManager;
    

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if(bgmManager != null)
        {
            bgmManager.PlayBGM(clip);
        }
    }

    public void StopBGM()
    {
        if(bgmManager != null)
        {
            bgmManager.StopBGM();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 0.3f)
    {
        if(sfxManager != null)
        {
            sfxManager.PlaySFX(clip, volume);
        }
    }

    public static void PlayOneShot(GameObject target, AudioClip clip, AudioMixerGroup mixerGroup)
    {
        if (clip == null) return;

        AudioSource sfx = target.AddComponent<AudioSource>();
        sfx.clip = clip;
        sfx.outputAudioMixerGroup = mixerGroup;
        sfx.Play();
        Object.Destroy(sfx, clip.length);
    }
}