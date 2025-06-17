using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 슬롯(위치 기준점)을 사용하여 초상화를 정렬된 위치에서 정확하게 교환하는 퍼즐 매니저
/// </summary>
public class PortraitSwapperWithSlots : MonoBehaviour
{
    [Header("초상화 설정")]
    [SerializeField] private LayerMask portraitLayer;
    [SerializeField] private List<PortraitWithID> portraits;

    [Header("정답 순서 (ID 기준)")]
    [SerializeField] private int[] correctOrder;

    [Header("슬롯 위치 설정")]
    [SerializeField] private Transform[] slots;

    [Header("액자 설정")]
    [SerializeField] private GameObject fallingFrame;
    [SerializeField] private float fallGravityScale = 5.0f;
    [SerializeField] private float shakeDuration = 0.4f;
    [SerializeField] private float shakeSpeed = 30f;
    [SerializeField] private float shakeAngle = 5f;

    [Header("퍼즐 성공 시 바꿀 대상 오브젝트")]
    [SerializeField] private GameObject targetObjectToChange;
    [SerializeField] private Sprite replacementSprite;

    [Header("퍼즐 성공 시 활성화할 오브젝트들")]
    [SerializeField] private GameObject[] objectsToEnableOnSuccess;

    [Header("퍼즐 성공 시 비활성화할 오브젝트들")]
    [SerializeField] private GameObject[] objectsToDisableOnSuccess;

    private PortraitWithID firstSelected = null;
    private bool isSwapping = false;
    private bool isPuzzleCompleted = false;
    public string puzzleID = "Portrait";

    private float originalZRotation = 0f;
    private float originalZPosition = 0f;

    private void OnEnable()
    {
        // 슬롯 위치 초기화
        for (int i = 0; i < portraits.Count; i++)
        {
            portraits[i].SetIndex(i);
            portraits[i].transform.position = slots[i].position;
        }

        // fallingFrame 초기 상태 저장
        if (fallingFrame != null)
        {
            originalZRotation = fallingFrame.transform.eulerAngles.z;
            originalZPosition = fallingFrame.transform.position.z;
        }

        // 퍼즐 상태 복원
        StartCoroutine(InitializePortraitPuzzleState());
    }
    private IEnumerator InitializePortraitPuzzleState()
    {
        yield return new WaitUntil(() => PuzzleManager.Instance != null);
        yield return null;

        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            isPuzzleCompleted = true;

            if (targetObjectToChange != null && replacementSprite != null)
            {
                SpriteRenderer sr = targetObjectToChange.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite = replacementSprite;
                }
            }

            if (objectsToEnableOnSuccess != null)
            {
                foreach (GameObject obj in objectsToEnableOnSuccess)
                {
                    if (obj != null)
                    {
                        obj.SetActive(true);
                    }
                }
            }

            if (objectsToDisableOnSuccess != null)
            {
                foreach (GameObject obj in objectsToDisableOnSuccess)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }
    }

    public void OnPortraitClicked(PortraitWithID clicked)
    {
        if (isSwapping || isPuzzleCompleted) return;

        if (firstSelected == null)
        {
            firstSelected = clicked;
        }
        else
        {
            if (clicked == firstSelected)
            {
                firstSelected = null;
                return;
            }

            StartCoroutine(SwapPortraits(firstSelected, clicked));
            firstSelected = null;
        }
    }

    IEnumerator SwapPortraits(PortraitWithID a, PortraitWithID b)
    {
        isSwapping = true;

        int indexA = a.CurrentIndex;
        int indexB = b.CurrentIndex;

        a.SetIndex(indexB);
        b.SetIndex(indexA);

        Vector3 targetA = slots[indexB].position;
        Vector3 targetB = slots[indexA].position;

        float duration = 0.3f;
        float elapsed = 0f;

        Vector3 startA = a.transform.position;
        Vector3 startB = b.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);

            a.transform.position = Vector3.Lerp(startA, targetA, t);
            b.transform.position = Vector3.Lerp(startB, targetB, t);
            yield return null;
        }

        a.transform.position = targetA;
        b.transform.position = targetB;

        isSwapping = false;
        CheckAnswer();
    }

    void CheckAnswer()
    {
        portraits.Sort((a, b) => a.CurrentIndex.CompareTo(b.CurrentIndex));

        for (int i = 0; i < portraits.Count; i++)
        {
            if (portraits[i].ID != correctOrder[i])
            {
                Debug.Log("❌ 정답 아님");
                return;
            }
        }

        Debug.Log("🎉 정답!");
        isPuzzleCompleted = true;
        StartCoroutine(ShakeAndDropFrame());

        // ✅ 퍼즐 성공 시 타겟 오브젝트의 스프라이트 교체
        if (targetObjectToChange != null && replacementSprite != null)
        {
            SpriteRenderer sr = targetObjectToChange.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = replacementSprite;
                Debug.Log("✅ 타겟 오브젝트의 스프라이트가 교체되었습니다.");
            }
        }

        // ✅ 퍼즐 성공 시 오브젝트들 활성화
        if (objectsToEnableOnSuccess != null)
        {
            foreach (GameObject obj in objectsToEnableOnSuccess)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                    Debug.Log("✅ 오브젝트 활성화됨: " + obj.name);
                }
            }
        }

        // ✅ 퍼즐 성공 시 오브젝트들 비활성화
        if (objectsToDisableOnSuccess != null)
        {
            foreach (GameObject obj in objectsToDisableOnSuccess)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    Debug.Log("🚫 오브젝트 비활성화됨: " + obj.name);
                }
            }
        }

        PuzzleManager.Instance.CompletePuzzle(puzzleID);
    }

    IEnumerator ShakeAndDropFrame()
    {
        if (fallingFrame == null) yield break;

        Transform frame = fallingFrame.transform;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float angle = Mathf.Sin(elapsed * shakeSpeed) * shakeAngle;
            frame.rotation = Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }

        frame.rotation = Quaternion.Euler(0f, 0f, originalZRotation);

        Vector3 pos = frame.position;
        frame.position = new Vector3(pos.x, pos.y, originalZPosition);

        yield return new WaitForSeconds(0.1f);

        Rigidbody2D rb = fallingFrame.AddComponent<Rigidbody2D>();
        rb.gravityScale = fallGravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
