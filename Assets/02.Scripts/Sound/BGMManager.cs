using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BGMManager : MonoBehaviour
{
    private AudioSource bgmSource;

    [Header("BGM ����")]
    public AudioMixerGroup bgmMixerGroup;
    public float fadeDuration = 1.0f;

    private void Awake()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        if (bgmMixerGroup != null)
        {
            bgmSource.outputAudioMixerGroup = bgmMixerGroup;
        }
    }

    // �ܺο��� BGM�� ����ϰ� ���� �� ���
    public void PlayBGM(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("����� BGM�� �����ϴ�.");
            return;
        }

        if (bgmSource.isPlaying)
        {
            StartCoroutine(FadeOutIn(clip));
        }
        else
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
    }

    private IEnumerator FadeOutIn(AudioClip newClip)
    {
        float t = 0f;
        float startVolume = bgmSource.volume;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }
    }
}