using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    public Item item;

    // 텍스트 패널 제어용 변수 추가
    public FloatingTextUI floatingTextUI;

    // 마우스로 클릭했을 때 호출됨
    private void OnMouseDown()
    {
        if (floatingTextUI != null)
        {
            floatingTextUI.ShowPanel(transform); // 자신의 위치 전달
        }
    }

    public Item ClickItem()
    {
        return this.item;
    }
}
