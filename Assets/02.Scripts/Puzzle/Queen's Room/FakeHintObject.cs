using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHintObject : MonoBehaviour, IObjectItem
{
    public Item fakeHintItem;    // 인벤토리에 들어갈 쪽지 아이템
    public int nextStoryIndex = 136;

    public Item ClickItem()
    {
        // 인벤토리에 쪽지가 없는 경우만 추가
        bool alreadyInInventory = Inventory.Instance.items.Exists(x => x == fakeHintItem);
        if (!alreadyInInventory)
        {
            Inventory.Instance.AddItem(fakeHintItem);
        }

        // 오브젝트 비활성화
        gameObject.SetActive(false);

        // 씬 전환 (스토리 136)
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));

        return null;
    }
}