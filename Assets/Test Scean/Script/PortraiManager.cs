using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraiManager : MonoBehaviour
{
    [Header("💠 퍼즐 슬롯들")]
    public BookSlot[] allSlots;

    [Header("🎯 퍼즐 성공 시 활성화할 오브젝트")]
    public GameObject objectToActivateOnSuccess;

    [Header("🖼️ 퍼즐 성공 시 교체할 배경 오브젝트")]
    public GameObject backgroundObject;
    public Sprite newBackgroundSprite;

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
        // ✅ 오브젝트 활성화
        if (objectToActivateOnSuccess != null)
        {
            objectToActivateOnSuccess.SetActive(true);

            SpriteRenderer sr = objectToActivateOnSuccess.GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = true;

            Debug.Log("🏆 퍼즐 성공! 오브젝트가 활성화됨: " + objectToActivateOnSuccess.name);
        }
        else
        {
            Debug.LogWarning("⚠️ 활성화할 오브젝트가 비어 있습니다.");
        }

        // ✅ 배경 스프라이트 교체 추가
        if (backgroundObject != null && newBackgroundSprite != null)
        {
            SpriteRenderer bgSr = backgroundObject.GetComponent<SpriteRenderer>();
            if (bgSr != null)
            {
                bgSr.sprite = newBackgroundSprite;
                Debug.Log("🖼️ 배경 이미지가 새 이미지로 교체되었습니다!");
            }
        }
    }
}
