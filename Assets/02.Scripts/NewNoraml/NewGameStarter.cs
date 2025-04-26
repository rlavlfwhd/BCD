using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameStarter : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneDataManager.Instance.Data = new SceneData();
        Debug.Log("New Game 시작 (독립 스크립트에서)");

        // PlayScene으로 전환
        SceneManager.LoadScene("StoryScene");
    }
}
