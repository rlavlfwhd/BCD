using System.Collections.Generic;

[System.Serializable]
public class SceneData
{
    public int currentStoryIndex = 1;
    [System.NonSerialized]
    public int nextStoryIndex = 0;

    public List<string> completedPuzzles = new List<string>();
    public List<string> inventoryItemNames = new List<string>();
    public List<string> acquiredItemIDs = new List<string>();
    public List<string> createdRopes = new List<string>();    
    public List<int> seenStoryNumbers = new List<int>();
}