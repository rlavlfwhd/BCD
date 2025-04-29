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
    }

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if(SoundManager.instance != null)
        {
            if (sceneBGMMap.ContainsKey(sceneName))
            {
                SoundManager.instance.PlayBGM(sceneBGMMap[sceneName]);
            }
            else
            {

                SoundManager.instance.PlayBGM(defaultBGM);
            }
        }
    }
}
