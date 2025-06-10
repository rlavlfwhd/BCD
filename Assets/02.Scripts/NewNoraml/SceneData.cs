using System.Collections.Generic;

[System.Serializable]
public class SceneData
{
    public int currentStoryIndex = 1;
    [System.NonSerialized]
    public int nextStoryIndex = 0;

    public List<string> completedPuzzles = new List<string>();    
    public List<string> acquiredItemIDs = new List<string>();
    public List<string> createdRopes = new List<string>();    
    public List<int> seenStoryNumbers = new List<int>();
    public List<string> mirrorItemGiven = new List<string>();
    public List<int> acquiredLetterPieces = new List<int>(); // 편지조각 번호
    public bool jewelsGiven = false;
    public bool letterJewelsGiven = false;  // 추가
}