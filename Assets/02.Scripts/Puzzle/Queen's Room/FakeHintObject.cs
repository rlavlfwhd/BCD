using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeHintObject : MonoBehaviour, IObjectItem
{
    public Item fakeHintItem;    // �κ��丮�� �� ���� ������
    public int nextStoryIndex = 136;

    public Item ClickItem()
    {
        // �κ��丮�� ������ ���� ��츸 �߰�
        bool alreadyInInventory = Inventory.Instance.items.Exists(x => x == fakeHintItem);
        if (!alreadyInInventory)
        {
            Inventory.Instance.AddItem(fakeHintItem);
        }

        // ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);

        // �� ��ȯ (���丮 136)
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));

        return null;
    }
}