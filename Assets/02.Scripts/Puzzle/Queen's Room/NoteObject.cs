using UnityEngine;
using UnityEngine.SceneManagement;

public class NoteObject : MonoBehaviour, IObjectItem
{
    public Item noteItem;    // 인벤토리에 들어갈 쪽지 아이템
    public int nextStoryIndex = 136;

    public Item ClickItem()
    {
        // 인벤토리에 쪽지가 없는 경우만 추가
        bool alreadyInInventory = Inventory.Instance.items.Exists(x => x == noteItem);
        if (!alreadyInInventory)
        {
            Inventory.Instance.AddItem(noteItem);
        }

        // 오브젝트 비활성화
        gameObject.SetActive(false);

        // 씬 전환 (스토리 136)
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        FadeManager.Instance.StartCoroutine(FadeManager.Instance.FadeToStoryScene("StoryScene"));

        return null;
    }
}
