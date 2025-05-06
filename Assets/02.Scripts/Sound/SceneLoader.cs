using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("¾Àº° BGM ¼³Á¤")]
    public AudioClip storySceneBGM;
    public AudioClip pWindowSceneBGM;
    public AudioClip pBookshelfSceneBGM;
    public AudioClip mainSceneBGM;
    public AudioClip defaultBGM;

    private Dictionary<string, AudioClip> sceneBGMMap;

    private void Awake()
    {
        sceneBGMMap = new Dictionary<string, AudioClip>()
        {
            { "StoryScene", storySceneBGM},
            { "PWindowScene", pWindowSceneBGM},
            { "PBookshelfScene", pBookshelfSceneBGM},
            { "MainScene", mainSceneBGM }
        };

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        Debug.Log("¾À ÀüÈ¯ °¨Áö: " + sceneName); // µð¹ö±ë¿ë

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