using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public BookSlot[] slots;
    public GameObject backgroundObject;
    public GameObject Door;
    public Item chickenStatueItem;

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
        float distance = 1200f;
        float speed = 160f; // 초당 이동 속도
        float duration = distance / speed;

        float elapsed = 0f;

        Vector3 startPos = backgroundObject.transform.position;
        Vector3 endPos = startPos + new Vector3(distance, 0, 0);

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
