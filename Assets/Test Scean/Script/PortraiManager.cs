using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraiManager : MonoBehaviour
{
    [Header("💠 퍼즐 슬롯들")]
    public BookSlot[] allSlots;

    [Header("🎯 퍼즐 성공 시 활성화할 오브젝트")]
    public GameObject objectToActivateOnSuccess; // 🎯 apt 오브젝트 할당 예정

    private bool isPuzzleCompleted = false;

    public void CheckSlotsNow()
    {
        if (isPuzzleCompleted) return;

        foreach (BookSlot slot in allSlots)
        {
            if (!slot.isCorrect)
            {
                Debug.Log("❌ 아직 맞지 않은 슬롯이 있어!");
                return;
            }
        }

        Debug.Log("🎯 모든 슬롯 정답!");
        isPuzzleCompleted = true;
        TriggerReward();
    }

    void TriggerReward()
    {
        if (objectToActivateOnSuccess != null)
        {
            objectToActivateOnSuccess.SetActive(true);

            // SpriteRenderer 강제 표시 보장
            SpriteRenderer sr = objectToActivateOnSuccess.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;

            Debug.Log("🏆 퍼즐 성공! 오브젝트가 활성화되었습니다: " + objectToActivateOnSuccess.name);
        }
        else
        {
            Debug.LogWarning("⚠️ 활성화할 오브젝트가 비어 있습니다.");
        }
    }
}
