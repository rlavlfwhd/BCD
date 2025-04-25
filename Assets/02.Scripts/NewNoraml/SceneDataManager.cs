using System.Collections.Generic;
using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public static SceneDataManager Instance;
    public SceneData Data = new SceneData();

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
}