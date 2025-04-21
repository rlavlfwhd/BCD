using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowPuzzle : MonoBehaviour, IDropTarget
{
    public Item neededItem; // ����
    public Sprite openedWindowSprite;
    public Image windowImage; // â�� �̹��� ���� ���
    public GameObject clickableWindowObject; // Ŭ�� ������ â�� ������Ʈ (�ʱ⿡ ��Ȱ��ȭ)

    public string puzzleID = "window_rope";

    public void OnItemDropped(Item item)
    {
        if (item == neededItem && !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            if (!SceneDataManager.Instance.Data.isRopeUsed)
            {
                Inventory.Instance.RemoveItemByName("Rope4");
                SceneDataManager.Instance.Data.isRopeUsed = true;
                Debug.Log("Rope4 ��� �Ϸ�!");
            }

            PuzzleManager.Instance.CompletePuzzle(puzzleID);
            windowImage.sprite = openedWindowSprite;
            clickableWindowObject.SetActive(true); // Ŭ�� ������ ���� â�� ����
            Debug.Log("â�� ����!");
        }
    }
}
