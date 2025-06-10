using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPieceObject : MonoBehaviour, IObjectItem
{
    public int pieceNumber;        // 0~3
    public Item letterItem;        // 편지 아이템

    public Item ClickItem()
    {
        var data = SceneDataManager.Instance.Data;

        // 이미 조각 얻었으면 아무것도 안 함
        if (!data.acquiredLetterPieces.Contains(pieceNumber))
        {
            data.acquiredLetterPieces.Add(pieceNumber);

            // 인벤토리에 편지 없는 경우에만 추가
            bool alreadyInInventory = Inventory.Instance.items.Exists(x => x == letterItem);
            if (!alreadyInInventory)
            {
                Inventory.Instance.AddItem(letterItem);
            }
        }

        // 조각 오브젝트 비활성화
        gameObject.SetActive(false);

        // 반환값 필요없음
        return null;
    }
}