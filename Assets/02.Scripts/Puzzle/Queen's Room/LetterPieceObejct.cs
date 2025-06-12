using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPieceObject : MonoBehaviour, IObjectItem
{
    public int pieceNumber;        // 0~3
    public Item letterItem;        // ���� ������

    private void OnEnable()
    {
        var data = SceneDataManager.Instance != null ? SceneDataManager.Instance.Data : null;
        if (data != null && data.acquiredLetterPieces.Contains(pieceNumber))
        {
            gameObject.SetActive(false);
        }
    }

    public Item ClickItem()
    {
        var data = SceneDataManager.Instance.Data;

        // �̹� ���� ������� �ƹ��͵� �� ��
        if (!data.acquiredLetterPieces.Contains(pieceNumber))
        {
            data.acquiredLetterPieces.Add(pieceNumber);

            // �κ��丮�� ���� ���� ��쿡�� �߰�
            bool alreadyInInventory = Inventory.Instance.items.Exists(x => x == letterItem);
            if (!alreadyInInventory)
            {
                Inventory.Instance.AddItem(letterItem);
            }
        }

        // ���� ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);

        // ��ȯ�� �ʿ����
        return null;
    }
}