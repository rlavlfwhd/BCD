using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableMirror : MonoBehaviour
{
    public Item pendantItem; // 얻을 아이템: Pendant
    public GameObject mirrorPanel;

    private bool isItemGiven = false; // 중복 획득 방지

    void OnMouseDown()
    {
        if (!isItemGiven)
        {
            Inventory.Instance.AddItem(pendantItem); // 인벤토리에 Pendant 추가
            isItemGiven = true;
            Debug.Log("Pendant 아이템 획득!");

            
        }
        else
        {
            mirrorPanel.SetActive(false);
            Debug.Log("이미 Pendant를 획득했습니다!");
        }
    }
}