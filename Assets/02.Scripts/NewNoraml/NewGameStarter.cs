using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameStarter : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneDataManager.Instance.Data = new SceneData();
        Debug.Log("New Game ���� (���� ��ũ��Ʈ����)");

        // PlayScene���� ��ȯ
        SceneManager.LoadScene("StoryScene");
    }
}
