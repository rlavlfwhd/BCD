using System.Collections.Generic;

[System.Serializable]
public class SceneData
{
    public int currentStoryIndex = 1;
    public int nextStoryIndex = 0;
    public bool isWindowPuzzleSolved = false;
    public bool isMirrorFlipped = false;
    public bool isRopeCrafted = false;
    public bool isRopeUsed = false;

    // 추가 가능한 상태들 (필요 시 확장)
    public HashSet<string> completedPuzzles = new HashSet<string>();
    public List<string> inventoryItemNames = new List<string>();
    public List<string> acquiredItemIDs = new List<string>();
    public HashSet<string> createdRopes = new HashSet<string>();
    public HashSet<string> usedRopes = new HashSet<string>();
}