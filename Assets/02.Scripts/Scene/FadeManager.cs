using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    public CanvasGroup fadeImage;
    public GameObject loadingPanel;
    public TextMeshProUGUI loadingText;

    float fadeDuration = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImage.DOFade(0, fadeDuration)
                .OnComplete(() => {
                    fadeImage.blocksRaycasts = false;

                    if (loadingPanel != null)
                    {
                        loadingPanel.SetActive(false);
                    }
                });
    }

    public void ChangeScene(string sceneName)
    {
        fadeImage.DOFade(1, fadeDuration)
            .OnStart(() => {
                fadeImage.blocksRaycasts = true;

                if (loadingPanel != null)
                {
                    loadingPanel.SetActive(true);
                }
            })
            .OnComplete(() => {
                StartCoroutine(LoadScene(sceneName));
            });
    }

    IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (!(async.isDone))
        {
            yield return null;

            if (async.progress >= 0.9f)
            {
                async.allowSceneActivation = true;
            }
        }
    }
}