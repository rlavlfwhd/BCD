using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        public int currentStoryIndex;
        public string mainImagePath;
        public string mainImagePath2;
        public string puzzleImagePath;
        public string sceneName;
        public string savedTime;

        public List<string> inventoryItemNames;
        public List<string> completedPuzzles;
        public SceneData sceneState;
    }

    public static void SaveGame(int slot, int storyIndex, string imagePath, string imagePath2, List<string> inventoryItems, List<string> completedPuzzles)
    {
        SaveData saveData = new SaveData
        {
            currentStoryIndex = storyIndex,
            mainImagePath = imagePath,
            mainImagePath2 = imagePath2,
            inventoryItemNames = inventoryItems,
            completedPuzzles = completedPuzzles,
            sceneState = SceneDataManager.Instance.Data,
            sceneName = SceneManager.GetActiveScene().name,
            savedTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        if (saveData.sceneName.StartsWith("P"))
        {
            PuzzleSceneBackground bg = GameObject.FindObjectOfType<PuzzleSceneBackground>();
            if (bg != null && bg.backgroundSprite != null)
            {
                Texture2D texture = bg.backgroundSprite.texture;
                byte[] pngData = texture.EncodeToPNG();
                string puzzleImagePath = Application.persistentDataPath + $"/puzzleSlot{slot}_image.png"; // �� ���� �̸� �ٲ�!
                File.WriteAllBytes(puzzleImagePath, pngData);
                saveData.puzzleImagePath = puzzleImagePath;

                Debug.Log($"����� ��� �̹��� ���� �Ϸ�: {puzzleImagePath}");
            }
            else
            {
                Debug.LogWarning("����� ��� �̹����� �������� �ʾҽ��ϴ�!");
            }
        }

        string json = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/saveSlot" + slot + ".json";
        File.WriteAllText(path, json);
    }

    public static string CapturePuzzleSceneScreenshot(int slot)
    {
        string path = Application.persistentDataPath + $"/puzzleSlot{slot}_image.png";
        ScreenCapture.CaptureScreenshot(path);
        Debug.Log($"����� ĸó ���� �Ϸ�: {path}");
        return path;
    }

    public static SaveData LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/saveSlot" + slot + ".json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            // �� ���� ����
            if (saveData.sceneState != null)
            {
                SceneDataManager.Instance.Data = saveData.sceneState;
            }

            return saveData;
        }
        else
        {
            return null;
        }
    }
}