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
        public int choice1Count;
        public int choice2Count;
        public int choice3Count;
        public string mainImagePath;
    }

    public static void SaveGame(int slot, int stroyIndex, int choice1, int choice2, int choice3, string imagePath)
    {
        SaveData saveData = new SaveData
        {
            currentStoryIndex = stroyIndex,
            choice1Count = choice1,
            choice2Count = choice2,
            choice3Count = choice3,
            mainImagePath = imagePath
        };

        string json = JsonUtility.ToJson(saveData, true);
        string path = Application.persistentDataPath + "/saveSlot" + slot + "json";
        File.WriteAllText(path, json);
    }

    public static SaveData LoadGame(int slot)
    {
        string path = Application.persistentDataPath + "/saveSlot" + slot + "json";

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
