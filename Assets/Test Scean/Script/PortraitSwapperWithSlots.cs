using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 슬롯(위치 기준점)을 사용하여 초상화를 정렬된 위치에서 정확하게 교환하는 퍼즐 매니저
/// </summary>
public class PortraitSwapperWithSlots : MonoBehaviour
{
    [Header("초상화 설정")]
    [SerializeField] private LayerMask portraitLayer; // 클릭 감지용 레이어
    [SerializeField] private List<PortraitWithID> portraits; // 현재 초상화들

    [Header("정답 순서 (ID 기준)")]
    [SerializeField] private int[] correctOrder; // 정답 순서 ID 배열

    [Header("슬롯 위치 설정")]
    [SerializeField] private Transform[] slots; // 미리 배치된 슬롯 위치들 (빈 오브젝트, 콜라이더 필요 없음)

    [Header("액자 설정")]
    [SerializeField] private GameObject fallingFrame; // 떨어질 액자 오브젝트
    [SerializeField] private float fallGravityScale = 5.0f; // 낙하 중력 세기
    [SerializeField] private float shakeDuration = 0.4f; // 흔들리는 시간
    [SerializeField] private float shakeSpeed = 30f; // 흔들림 속도
    [SerializeField] private float shakeAngle = 5f; // 흔들림 각도

    private PortraitWithID firstSelected = null; // 첫 번째로 선택된 초상화
    private bool isSwapping = false; // 현재 교환 중인지 여부
    private bool isPuzzleCompleted = false; // 퍼즐이 완료되었는지 여부

    private float originalZRotation = 0f; // 액자 원래 회전 Z
    private float originalZPosition = 0f; // 액자 원래 위치 Z

    private void Start()
    {
        for (int i = 0; i < portraits.Count; i++)
        {
            portraits[i].SetIndex(i);
            portraits[i].transform.position = slots[i].position;
        }

        if (fallingFrame != null)
        {
            originalZRotation = fallingFrame.transform.eulerAngles.z;
            originalZPosition = fallingFrame.transform.position.z;
        }
    }

    void Update()
    {
        if (isSwapping || isPuzzleCompleted) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, portraitLayer);

            if (hit.collider != null)
            {
                PortraitWithID clicked = hit.collider.GetComponent<PortraitWithID>();

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

        // 원래 회전 복원
        frame.rotation = Quaternion.Euler(0f, 0f, originalZRotation);

        // Z 위치 복원 (혹시라도 변화가 있었다면)
        Vector3 pos = frame.position;
        frame.position = new Vector3(pos.x, pos.y, originalZPosition);

        yield return new WaitForSeconds(0.1f);

        Rigidbody2D rb = fallingFrame.AddComponent<Rigidbody2D>();
        rb.gravityScale = fallGravityScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}