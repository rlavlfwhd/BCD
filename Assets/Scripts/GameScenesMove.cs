using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScenesMove : MonoBehaviour
{
    public void GameScnesCtrl()
    {
        SceneManager.LoadScene("Z6.item_record"); //어떤 씬으로 이동할 건지
        Debug.Log("Game Scenes go");
    }
    // Start is called before the first frame update
}
