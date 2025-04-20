using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    public Item item;

    // �ؽ�Ʈ �г� ����� ���� �߰�
    public FloatingTextUI floatingTextUI;

    // ���콺�� Ŭ������ �� ȣ���
    private void OnMouseDown()
    {
        if (floatingTextUI != null)
        {
            floatingTextUI.ShowPanel(transform); // �ڽ��� ��ġ ����
        }
    }

    public Item ClickItem()
    {
        return this.item;
    }
}
