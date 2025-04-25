using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleObject : MonoBehaviour
{
    public string puzzleID = "rope_puzzle";
    public Item neededItem;
    public Sprite newSprite;
    public Image targetImage;
    public int nextStoryIndex;

    public string puzzleFlagName = "rope_puzzle";

    public void OnItemDropped(Item item)
    {
        if (item == neededItem && !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            PuzzleManager.Instance.HandlePuzzleSuccess(targetImage, newSprite, nextStoryIndex, puzzleID);
        }
        else
        {
            Debug.Log("퍼즐 실패 혹은 이미 완료됨");
        }
    }
}

