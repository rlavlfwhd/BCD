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
            Debug.Log("�ùٸ� ������ ���!");
            // ��: ���� ������, ��ȣ�ۿ� �Ͼ
        }
        else
        {
            Debug.Log("�̰� �� �´� �������̾�");
        }
    }
}

