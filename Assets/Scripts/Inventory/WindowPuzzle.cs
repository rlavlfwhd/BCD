using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowPuzzle : MonoBehaviour, IDropTarget
{
    public Item neededItem; // 밧줄
    public Sprite openedWindowSprite;
    public Image windowImage; // 창문 이미지 변경 대상
    public GameObject clickableWindowObject; // 클릭 가능한 창문 오브젝트 (초기에 비활성화)

    public string puzzleID = "window_rope";

    public void OnItemDropped(Item item)
    {
        if (item == neededItem && !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            PuzzleManager.Instance.CompletePuzzle(puzzleID);
            windowImage.sprite = openedWindowSprite;
            clickableWindowObject.SetActive(true); // 클릭 가능한 열린 창문 등장
            Debug.Log("창문 열림!");
        }
    }
}
