using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio; // Audio ���� ����� ����ϱ� ���� �߰�

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance; // �̱��� �ν��Ͻ�

    public List<Sound> sounds = new List<Sound>(); // ���� ����Ʈ
    public AudioMixer audioMixer; // ����� �ͼ� ����

    void Awake()
    {
        // �̱��� ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ���带 �ʱ�ȭ�մϴ�.
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.mixerGroup; // ����� �ͼ� �׷� ����
        }
    }

    // ���带 ����ϴ� �޼���
    public void PlaySound(string name)
    {
        Sound soundToPlay = sounds.Find(sound => sound.name == name);
        if (soundToPlay != null)
        {
            soundToPlay.source.Play();
        }
        else
        {
            Debug.LogWarning("���� '" + name + "'�� ã�� �� �����ϴ�.");
        }
    }
}

[System.Serializable]
public class Sound
{
    public string name; // ������ �̸�
    public AudioClip clip; // ���� Ŭ��
    [Range(0f, 1f)]
    public float volume = 1f; // ���� ����
    [Range(0.1f, 3f)]
    public float pitch = 1f; // ���� ��ġ
    public bool loop; // �ݺ� ��� ����
    public AudioMixerGroup mixerGroup; // ����� �ͼ� �׷�

    [HideInInspector]
    public AudioSource source; // ����� �ҽ�
}