using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraiManager : MonoBehaviour
{
    [Header("💠 퍼즐 슬롯들")]
    public BookSlot[] allSlots;

    [Header("🎯 퍼즐 성공 시 활성화할 오브젝트")]
    public GameObject objectToActivateOnSuccess;

    [Header("🖼️ 교체할 스프라이트")]
    public Sprite firstSprite;          // 퍼즐 성공 직후 보여줄 이미지
    public Sprite secondSprite;         // 몇 초 후 교체될 이미지

    [Header("🕐 두 번째 이미지로 교체될 시간 (초)")]
    public float delayBeforeSecondSprite = 2f;

    [Header("🌄 배경 이미지 관련")]
    public GameObject backgroundObject;
    public Sprite newBackgroundSprite;

    [Header("✨ 퍼즐 성공 시 활성화할 오브젝트 목록")]
    public GameObject[] objectsToEnableOnSuccess;

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

            SpriteRenderer sr = objectToActivateOnSuccess.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                if (firstSprite != null)
                {
                    sr.sprite = firstSprite;
                    sr.enabled = true;
                    Debug.Log("🏆 퍼즐 성공! 첫 번째 이미지 적용됨: " + firstSprite.name);
                }

                if (secondSprite != null)
                {
                    StartCoroutine(SwapToSecondSpriteAfterDelay(sr));
                }
            }
            else
            {
                Debug.LogWarning("⚠️ SpriteRenderer가 대상 오브젝트에 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 활성화할 오브젝트가 비어 있습니다.");
        }

        // ✅ 배경 스프라이트 교체
        if (backgroundObject != null && newBackgroundSprite != null)
        {
            SpriteRenderer bgSr = backgroundObject.GetComponent<SpriteRenderer>();
            if (bgSr != null)
            {
                bgSr.sprite = newBackgroundSprite;
                Debug.Log("🖼️ 배경 이미지가 새 이미지로 교체되었습니다!");
            }
            else
            {
                Debug.LogWarning("⚠️ backgroundObject에 SpriteRenderer가 없습니다.");
            }
        }

        // ✅ 추가된 오브젝트 활성화 기능
        if (objectsToEnableOnSuccess != null && objectsToEnableOnSuccess.Length > 0)
        {
            foreach (GameObject go in objectsToEnableOnSuccess)
            {
                if (go != null)
                {
                    go.SetActive(true);
                    Debug.Log("✅ 추가 오브젝트 활성화됨: " + go.name);
                }
            }
        }
    }

    IEnumerator SwapToSecondSpriteAfterDelay(SpriteRenderer sr)
    {
        yield return new WaitForSeconds(delayBeforeSecondSprite);

        sr.sprite = secondSprite;
        Debug.Log("🔄 두 번째 이미지로 교체됨: " + secondSprite.name);
    }
}
