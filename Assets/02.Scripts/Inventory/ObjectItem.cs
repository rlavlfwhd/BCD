using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    public Item item;

    [Header("사운드 매니저에 등록된 이름")]
    public string soundName; // 🎯 SoundManager의 사운드 이름으로 연결

    private void OnMouseDown()
    {
        ClickItem();
    }

    public Item ClickItem()
    {
        Debug.Log("📣 ClickItem 호출됨!");

        // ✅ SoundManager를 통해 소리 재생
        if (!string.IsNullOrEmpty(soundName))
        {
            SoundManager.instance.PlaySound(soundName);
        }

        return this.item;
    }
}