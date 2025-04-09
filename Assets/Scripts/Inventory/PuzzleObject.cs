using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleObject : MonoBehaviour, IDropTarget
{
    public Item neededItem;

    public void OnItemDropped(Item item)
    {
        if (item == neededItem)
        {
            Debug.Log("올바른 아이템 사용!");
            // 예: 문이 열린다, 상호작용 일어남
        }
        else
        {
            Debug.Log("이건 안 맞는 아이템이야");
        }
    }
}

