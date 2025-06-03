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
        StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));
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
        SceneDataManager.Instance.Data.currentStoryIndex = StorySystem.Instance.currentStoryIndex;

        List<string> itemNames = new List<string>();
        foreach (var item in Inventory.Instance.items)
        {
            itemNames.Add(item.itemName);
        }

        SaveSystem.SaveGame(slot, StorySystem.Instance != null ? StorySystem.Instance.currentStoryIndex : 1, imagePath, imagePath2, itemNames, PuzzleManager.Instance.GetCompletedPuzzleList());
    }

    public void LoadGame(int slot)
    {
        StartCoroutine(LoadGameWithFade(slot));
    }

    private IEnumerator LoadGameWithFade(int slot)
    {
        var saveData = SaveSystem.LoadGame(slot);
        if (saveData == null) yield break;

        
        SceneDataManager.Instance.Data = new SceneData();
        SceneDataManager.Instance.Data = saveData.sceneState;

        // 3. 씬 전환(필요시)
        if (saveData.sceneName == "StoryScene")
        {
            // 스토리씬(스토리번호 기준 챕터 연출)
            yield return FadeManager.Instance.FadeToLoadStoryScene("StoryScene");
        }
        else
        {
            // 퍼즐씬(씬 이름 기준 챕터 연출)
            yield return FadeManager.Instance.FadeToChoiceScene(saveData.sceneName);
        }

        // 씬 완전히 전환되고 한 프레임 대기
        yield return null;


        // 5. 게임 상태 복원
        yield return DeferredLoadAfterScene(saveData);
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
        SceneDataManager.Instance.Data.currentStoryIndex = storyIndex;
        StorySystem.Instance.StoryShow(storyIndex);
    }    
}