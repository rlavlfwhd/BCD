using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour
{
    [Header("SFX ����")]
    public AudioMixerGroup sfxMixerGroup;

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if(clip == null)
        {
            Debug.LogWarning("����� SFX�� �����ϴ�.");
            return;
        }

        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = Mathf.Clamp01(volume);

        if(sfxMixerGroup != null)
        {
            source.outputAudioMixerGroup = sfxMixerGroup;
        }

        
        source.Play();

        Destroy(source, clip.length);
    }
}
