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

    public void SaveGame(int slot)
    {
        if (slot < 1 || slot > 6) return;

        StoryModel currentStory = StorySystem.Instance.currentStoryModel;
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

        SaveSystem.SaveGame(slot, StorySystem.Instance.currentStoryIndex, imagePath, imagePath2, itemNames, PuzzleManager.Instance.GetCompletedPuzzleList());
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

        SceneDataManager.Instance.Data = new SceneData();
        SceneDataManager.Instance.Data = saveData.sceneState;

        if (saveData.sceneName.Contains("Puzzle") || saveData.sceneName == "TestScene") // 퍼즐씬 이름 기준
        {
            foreach (string id in SceneDataManager.Instance.Data.acquiredItemIDs)
            {
                GameObject[] all = GameObject.FindObjectsOfType<GameObject>(true);
                foreach (GameObject go in all)
                {
                    string clean = go.name.Replace("(Clone)", "").Trim();
                    if (clean == id.Trim())
                    {
                        go.SetActive(false);
                        break;
                    }
                }
            }
        }

        // 아이템/퍼즐 복원
        Inventory.Instance.items.Clear();
        foreach (var itemName in saveData.inventoryItemNames)
        {
            Item item = Resources.Load<Item>("Items/" + itemName);
            if (item != null)
                Inventory.Instance.items.Add(item);
        }
        Inventory.Instance.FreshSlot();

        PuzzleManager.Instance.SetCompletedPuzzleList(saveData.completedPuzzles);

        // PlayScene인 경우에만 StorySystem 사용
        if (saveData.sceneName == "PlayScene")
        {
            while (StorySystem.Instance == null || StorySystem.Instance.textComponent == null)
                yield return null;

            StorySystem.Instance.currentStoryIndex = saveData.currentStoryIndex;
            StorySystem.Instance.StoryShow(saveData.currentStoryIndex);
        }
    }

    private void RestoreFromSaveData(SaveSystem.SaveData saveData)
    {
        StorySystem.Instance.currentStoryIndex = saveData.currentStoryIndex;
        StorySystem.Instance.StoryShow(saveData.currentStoryIndex);

        Inventory.Instance.items.Clear();
        foreach (string itemName in saveData.inventoryItemNames)
        {
            Item loadedItem = Resources.Load<Item>("Items/" + itemName);
            if (loadedItem != null)
            {
                Inventory.Instance.items.Add(loadedItem);
            }
        }
        Inventory.Instance.FreshSlot();

        PuzzleManager.Instance.SetCompletedPuzzleList(saveData.completedPuzzles);
    }
}