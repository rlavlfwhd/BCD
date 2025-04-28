using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public BookSlot[] slots;
    public GameObject backgroundObject;
    public GameObject Door;
    public Item chickenStatueItem;

    [Header("사운드 매니저에 등록된 이름")]
    public string bookshelfMoveSoundName; // ✅ 책장이 이동할 때 재생할 사운드 이름

    private bool allSlotsCorrect = false;

    public void CheckSlotsNow()
    {
        if (!allSlotsCorrect && AreAllSlotsCorrect())
        {
            allSlotsCorrect = true;
            Debug.Log("🎉 모든 슬롯이 정답 책으로 채워졌습니다!");

            Inventory.Instance.AddItem(chickenStatueItem);
            Door.SetActive(true);
            StartCoroutine(SlideOutBookshelf());
        }
    }

    bool AreAllSlotsCorrect()
    {
        foreach (BookSlot slot in slots)
        {
            if (!slot.isOccupied || !slot.isCorrect)
                return false;
        }
        return true;
    }

    System.Collections.IEnumerator SlideOutBookshelf()
    {
        float distance = 1100f;
        float speed = 165f; // 초당 이동 속도
        float duration = distance / speed;

        float elapsed = 0f;

        Vector3 startPos = backgroundObject.transform.position;
        Vector3 endPos = startPos + new Vector3(distance, 0, 0);

        // ✅ 책장 이동 시작할 때 사운드 재생
        if (!string.IsNullOrEmpty(bookshelfMoveSoundName))
        {
            SoundManager.instance.PlaySound(bookshelfMoveSoundName);
        }

        while (elapsed < duration)
        {
            backgroundObject.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        backgroundObject.transform.position = endPos;
        backgroundObject.SetActive(false);
    }
}
