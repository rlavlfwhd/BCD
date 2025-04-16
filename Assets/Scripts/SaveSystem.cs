using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem
{
    [System.Serializable]
    public class SaveData
    {
        public int currentStoryIndex;
        public string mainImagePath;
        public string mainImagePath2;

        public List<string> inventoryItemNames;
        public List<string> completedPuzzles;
    }

    public static void SaveGame(int slot, int storyIndex, string imagePath, string imagePath2, List<string> inventoryItems, List<string> completedPuzzles)
    {
        SaveData saveData = new SaveData
        {
            currentStoryIndex = storyIndex,
            mainImagePath = imagePath,
            mainImagePath2 = imagePath2,
            inventoryItemNames = inventoryItems,
            completedPuzzles = completedPuzzles // Ãß°¡
        };


        string json = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/saveSlot" + slot + ".json";
        File.WriteAllText(path, json);
    }

    public static SaveData LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/saveSlot" + slot + ".json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            return saveData;
        }
        else
        {
            return null;
        }
    }
}
