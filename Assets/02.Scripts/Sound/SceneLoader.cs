using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("씬별 BGM 설정")]
    public AudioClip storySceneBGM;
    public AudioClip pWindowSceneBGM;
    public AudioClip pBookshelfSceneBGM;
    public AudioClip mainSceneBGM;
    public AudioClip defaultBGM;
    public AudioClip happyEndingBGM;
    public AudioClip badEndingBGM;

    private Dictionary<string, AudioClip> sceneBGMMap;

    private void Awake()
    {
        sceneBGMMap = new Dictionary<string, AudioClip>()
        {
            { "StoryScene", storySceneBGM},
            { "PWindowScene", pWindowSceneBGM},
            { "PBookshelfScene", pBookshelfSceneBGM},
            { "MainScene", mainSceneBGM },
            { "HappyEnding", happyEndingBGM},
            { "BadEnding", badEndingBGM},

        };

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        Debug.Log("씬 전환 감지: " + sceneName); // 디버깅용

        if (SoundManager.instance != null)
        {
            if (sceneBGMMap.TryGetValue(sceneName, out AudioClip clip))
            {
                SoundManager.instance.PlayBGM(clip);
            }
            else
            {
                SoundManager.instance.PlayBGM(defaultBGM);
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}