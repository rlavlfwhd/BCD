using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio; // Audio 관련 기능

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<Sound> sounds = new List<Sound>(); // 사운드 리스트
    public AudioMixer audioMixer; // 오디오 믹서 참조

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

        // 사운드 초기화
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.mixerGroup;
        }
    }

    public void PlaySound(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);
        if (soundToPlay != null)
        {
            // ✅ 수정된 부분: Play 전에 최신 Volume/Pitch 적용
            soundToPlay.source.volume = soundToPlay.volume;
            soundToPlay.source.pitch = soundToPlay.pitch;

            soundToPlay.source.Play();
        }
        else
        {
            Debug.LogWarning($"❗ 사운드 '{name}'를 찾을 수 없습니다.");
        }
    }
}

[System.Serializable]
public class Sound
{
    [Header("사운드 ID (정렬용 번호)")]
    public int id; // ✅ 추가: 사운드를 정렬/구분하기 위한 숫자 ID

    public string name; // 사운드 이름
    public AudioClip clip; // 오디오 클립
    [Range(0f, 1f)]
    public float volume = 1f; // 볼륨
    [Range(0.1f, 3f)]
    public float pitch = 1f; // 피치
    public bool loop; // 루프 여부
    public AudioMixerGroup mixerGroup; // 믹서 그룹

    [HideInInspector]
    public AudioSource source; // 런타임용 AudioSource
}
