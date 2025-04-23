using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableMirror : MonoBehaviour
{
    public Item pendantItem; // ���� ������: Pendant
    public GameObject mirrorPanel;

    private bool isItemGiven = false; // �ߺ� ȹ�� ����

    void OnMouseDown()
    {
        if (!isItemGiven)
        {
            Inventory.Instance.AddItem(pendantItem); // �κ��丮�� Pendant �߰�
            isItemGiven = true;
            Debug.Log("Pendant ������ ȹ��!");

            
        }
        else
        {
            mirrorPanel.SetActive(false);
            Debug.Log("�̹� Pendant�� ȹ���߽��ϴ�!");
        }
    }
}