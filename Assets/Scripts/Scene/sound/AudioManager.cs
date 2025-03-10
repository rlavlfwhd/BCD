using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip startMusic;
    public AudioClip book1GameMusic;

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

    private void Start()
    {
        PlayMusic(startMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void ChangeMusicForScene(string sceneName)
    {
        if (sceneName.StartsWith("Book1_GameScene"))
        {
            PlayMusic(book1GameMusic);
        }
        else
        {
            PlayMusic(startMusic);
        }
    }
}
