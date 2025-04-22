using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public BookSlot[] slots;                 // 슬롯 배열
    public GameObject backgroundObject;      // 배경 오브젝트 (책장 오브젝트)
    public GameObject Door;

    private bool allSlotsFilled = false;

    void Update()
    {
        if (!allSlotsFilled && AreAllSlotsFilled())
        {
            allSlotsFilled = true;
            Debug.Log("🎉 모든 슬롯이 채워졌습니다!");
            Door.SetActive(true);
            StartCoroutine(SlideOutBookshelf()); // 여기서 슬라이드 효과 호출
        }
    }

    bool AreAllSlotsFilled()
    {
        foreach (BookSlot slot in slots)
        {
            if (!slot.isOccupied) return false;
        }
        return true;
    }

    System.Collections.IEnumerator SlideOutBookshelf()
    {
        float duration = 12f;
        float elapsed = 0f;

        Vector3 startPos = backgroundObject.transform.position;
        Vector3 endPos = startPos + new Vector3(1500f, 0, 0); // 오른쪽으로 5 유닛 이동

        while (elapsed < duration)
        {
            backgroundObject.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        backgroundObject.transform.position = endPos;
        backgroundObject.SetActive(false); // 다 이동하면 비활성화        
    }
}
