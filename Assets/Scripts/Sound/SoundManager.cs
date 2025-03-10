using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio; // Audio 관련 기능을 사용하기 위해 추가

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // 싱글톤 인스턴스

    public List<Sound> sounds = new List<Sound>(); // 사운드 리스트
    public AudioMixer audioMixer; // 오디오 믹서 참조

    void Awake()
    {
        // 싱글톤 패턴 적용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 사운드를 초기화합니다.
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.mixerGroup; // 오디오 믹서 그룹 설정
        }
    }

    // 사운드를 재생하는 메서드
    public void PlaySound(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);
        if (soundToPlay != null)
        {
            soundToPlay.source.Play();
        }
        else
        {
            Debug.LogWarning("사운드 '" + name + "'를 찾을 수 없습니다.");
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name; // 사운드의 이름
    public AudioClip clip; // 사운드 클립
    [Range(0f, 1f)]
    public float volume = 1f; // 사운드 볼륨
    [Range(0.1f, 3f)]
    public float pitch = 1f; // 사운드 피치
    public bool loop; // 반복 재생 여부
    public AudioMixerGroup mixerGroup; // 오디오 믹서 그룹

    [HideInInspector]
    public AudioSource source; // 오디오 소스
}