using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Default SFX")]
    public AudioClip defaultButtonClickSFX;

    [Header("Default BGM")]
    public AudioClip defaultBGM;

    [Header("BGM 및 SFX 관리자")]
    public BGMManager bgmManager;
    public SFXManager sfxManager;

    // ──────────────────────────────────────────────────────────────────────────────
    // Life-cycle
    // ──────────────────────────────────────────────────────────────────────────────
    private void Awake()
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

    // ──────────────────────────────────────────────────────────────────────────────
    // BGM 제어
    // ──────────────────────────────────────────────────────────────────────────────
    public void PlayBGM(AudioClip clip)
    {
        if (bgmManager != null) bgmManager.PlayBGM(clip);
    }

    public void StopBGM()
    {
        if (bgmManager != null) bgmManager.StopBGM();
    }

    // ──────────────────────────────────────────────────────────────────────────────
    // SFX 제어
    // ──────────────────────────────────────────────────────────────────────────────
    public void PlaySFX(AudioClip clip, float volume = 0.3f)
    {
        if (sfxManager != null) sfxManager.PlaySFX(clip, volume);
    }

    // ──────────────────────────────────────────────────────────────────────────────
    // ① - 추천 버전 : 오브젝트 상태와 무관하게 **항상 끝까지** 재생됨
    // ──────────────────────────────────────────────────────────────────────────────
    public static void PlayOneShot(AudioClip clip,
                                   AudioMixerGroup mixerGroup,
                                   float volume = 1f)
    {
        if (clip == null) return;

        GameObject temp = new GameObject("TempSFX");
        AudioSource source = temp.AddComponent<AudioSource>();

        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.playOnAwake = false;
        source.volume = Mathf.Clamp01(volume);
        source.Play();

        Object.Destroy(temp, clip.length + 0.1f);   // 재생 끝난 뒤 자동 파괴
    }

    // ──────────────────────────────────────────────────────────────────────────────
    // ② - 기존 타깃 버전(필요 시 사용). 타깃이 비활성화되면 소리도 끊길 수 있음
    // ──────────────────────────────────────────────────────────────────────────────
    public static void PlayOneShot(GameObject target,
                                   AudioClip clip,
                                   AudioMixerGroup mixerGroup,
                                   float volume = 1f)
    {
        if (clip == null || target == null) return;

        AudioSource source = target.AddComponent<AudioSource>();

        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.playOnAwake = false;
        source.volume = Mathf.Clamp01(volume);
        source.Play();

        Object.Destroy(source, clip.length + 0.1f);
    }
}
