using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPieceObejct : MonoBehaviour, IObjectItem
{
    [Header("몇 번째 조각인지(1~4)")]
    public int pieceIndex;

    [Header("편지 아이템 (인벤토리 한 개만 존재)")]
    public Item letterItem; // 반드시 Inspector에서 "Letter" 할당

    public Item ClickItem()
    {
        // 인벤토리에 "Letter"가 없으면 추가
        if (!Inventory.Instance.items.Contains(letterItem))
        {
            Inventory.Instance.AddItem(letterItem);
        }
        // 획득 조각 등록
        LetterPieceManager.Instance.RegisterPiece(pieceIndex);

        // 효과음 등 처리 필요시 여기서

        return null; // 인벤토리에 편지 아이템 한 번만 들어가야 하므로 null 반환
    }
}
