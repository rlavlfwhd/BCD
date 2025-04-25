using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasCameraSetter : MonoBehaviour
{
    private Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            canvas.worldCamera = mainCam;
            Debug.Log("씬 로드 완료 → 카메라 연결 성공!");
        }
        else
        {
            Debug.LogError("씬 로드 후에도 카메라 못 찾음! MainCamera 태그 확인해봐!");
        }
    }
}