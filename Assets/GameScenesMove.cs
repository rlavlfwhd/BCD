using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScenesMove : MonoBehaviour
{
    public void GameScnesCtrl()
    {
        SceneManager.LoadScene("Z6.item_record"); //� ������ �̵��� ����
        Debug.Log("Game Scenes go");
    }
    // Start is called before the first frame update
}
