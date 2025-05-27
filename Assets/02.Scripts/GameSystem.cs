using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;

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

    public void NewGame()
    {
        SceneDataManager.Instance.Data = new SceneData();
        PuzzleManager.Instance.SetCompletedPuzzleList(new List<string>());
        SceneManager.LoadScene("StoryScene");
    }


    public void SaveGame(int slot)
    {
        if (slot < 1 || slot > 6) return;

        StoryModel currentStory = StorySystem.Instance != null ? StorySystem.Instance.currentStoryModel : null;
        string imagePath = "";
        string imagePath2 = "";

        if (currentStory != null)
        {
            if (currentStory.MainImage != null)
            {
                imagePath = Application.persistentDataPath + $"/slot{slot}_image1.png";
                File.WriteAllBytes(imagePath, currentStory.MainImage.EncodeToPNG());
            }

            if (currentStory.MainImage2 != null)
            {
                imagePath2 = Application.persistentDataPath + $"/slot{slot}_image2.png";
                File.WriteAllBytes(imagePath2, currentStory.MainImage2.EncodeToPNG());
            }
        }

        List<string> itemNames = new List<string>();
        foreach (var item in Inventory.Instance.items)
        {
            itemNames.Add(item.itemName);
        }

        SaveSystem.SaveGame(slot, StorySystem.Instance != null ? StorySystem.Instance.currentStoryIndex : 1, imagePath, imagePath2, itemNames, PuzzleManager.Instance.GetCompletedPuzzleList());
    }

    public void LoadGame(int slot)
    {
        var saveData = SaveSystem.LoadGame(slot);
        if (saveData == null) return;

        SceneDataManager.Instance.Data = new SceneData();
        SceneDataManager.Instance.Data = saveData.sceneState;

        if (SceneManager.GetActiveScene().name != saveData.sceneName)
        {
            SceneManager.LoadScene(saveData.sceneName);
            StartCoroutine(DeferredLoadAfterScene(saveData));
            return;
        }

        SceneManager.LoadScene(saveData.sceneName);
        StartCoroutine(DeferredLoadAfterScene(saveData));
    }

    private IEnumerator DeferredLoadAfterScene(SaveSystem.SaveData saveData)
    {
        yield return null;
        RestoreGameState(saveData);
    }

    public void RestoreGameState(SaveSystem.SaveData saveData)
    {
        if (saveData.sceneName == "StoryScene")
        {
            StartCoroutine(WaitForStorySystemAndShowStory(saveData.currentStoryIndex));
        }

        SceneDataManager.Instance.Data = saveData.sceneState;

        PuzzleManager.Instance.SetCompletedPuzzleList(saveData.completedPuzzles);

        Inventory.Instance.items.Clear();
        foreach (var itemName in saveData.inventoryItemNames)
        {
            Item item = Resources.Load<Item>("Items/" + itemName);
            if (item != null)
                Inventory.Instance.items.Add(item);
        }
        Inventory.Instance.FreshSlot();

        PuzzleManager.Instance.RestoreItemState();
    }

    IEnumerator WaitForStorySystemAndShowStory(int storyIndex)
    {
        while(StorySystem.Instance == null || StorySystem.Instance.textComponent == null)
        {
            Debug.Log("스토리 시스템 준비중 :)");
            yield return null;
        }

        StorySystem.Instance.currentStoryIndex = storyIndex;
        StorySystem.Instance.StoryShow(storyIndex);
    }
}