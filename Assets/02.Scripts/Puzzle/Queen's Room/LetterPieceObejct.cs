using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPieceObejct : MonoBehaviour, IObjectItem
{
    [Header("�� ��° ��������(1~4)")]
    public int pieceIndex;

    [Header("���� ������ (�κ��丮 �� ���� ����)")]
    public Item letterItem; // �ݵ�� Inspector���� "Letter" �Ҵ�

    public Item ClickItem()
    {
        // �κ��丮�� "Letter"�� ������ �߰�
        if (!Inventory.Instance.items.Contains(letterItem))
        {
            Inventory.Instance.AddItem(letterItem);
        }
        // ȹ�� ���� ���
        LetterPieceManager.Instance.RegisterPiece(pieceIndex);

        // ȿ���� �� ó�� �ʿ�� ���⼭

        return null; // �κ��丮�� ���� ������ �� ���� ���� �ϹǷ� null ��ȯ
    }
}
