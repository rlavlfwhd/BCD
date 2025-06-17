using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintObject : MonoBehaviour, IObjectItem
{
    public Item hintItem;    // �κ��丮�� �� ���� ������
    public int nextStoryIndex = 164;

    public Item ClickItem()
    {
        // �κ��丮�� ������ ���� ��츸 �߰�
        bool alreadyInInventory = Inventory.Instance.items.Exists(x => x == hintItem);
        if (!alreadyInInventory)
        {
            Inventory.Instance.AddItem(hintItem);
        }

        // ������Ʈ ��Ȱ��ȭ
        gameObject.SetActive(false);

        // �� ��ȯ (���丮 136)
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));

        return null;
    }
}