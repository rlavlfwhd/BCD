using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameStarter : MonoBehaviour
{
    public void StartNewGame()
    {
        GameSystem.Instance.NewGame();
        Debug.Log("¥∫∞‘¿”");
    }
}
