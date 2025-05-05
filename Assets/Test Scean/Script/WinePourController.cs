using System.Collections;
using UnityEngine;

/// <summary>
/// 🍷 와인 따르기 연출 담당 컨트롤러
/// 줄기 SpriteRenderer의 Y Scale을 조절하여 와인이 따르는 효과 연출
/// </summary>
public class WineStreamController : MonoBehaviour
{
    [Header("🎯 따르기 대상 줄기")]
    [Tooltip("와인 줄기 역할의 SpriteRenderer")]
    public SpriteRenderer wineStreamRenderer; // 줄기 SpriteRenderer 연결 (Inspector에서 드래그)

    [Header("🕐 따르는 시간 (초)")]
    [Tooltip("와인이 따르는 연출 시간 (길이 늘어나는 시간)")]
    public float pourDuration = 1.0f; // 따르기 지속 시간 (초)

    private void Start()
    {
        // 시작 시 와인 줄기 안 보이게 (scale y = 0으로)
        wineStreamRenderer.transform.localScale = new Vector3(1, 0, 1);
        // 오브젝트 비활성화
        wineStreamRenderer.gameObject.SetActive(false);
    }

    /// <summary>
    /// 🍷 따르기 연출 시작
    /// 호출 시 와인 줄기 길어졌다가 → 사라지는 애니메이션 진행
    /// </summary>
    public void StartPouring()
    {
        // 오브젝트 활성화 (보이게)
        wineStreamRenderer.gameObject.SetActive(true);
        // 코루틴 시작
        StartCoroutine(PourRoutine());
    }

    /// <summary>
    /// 🍷 따르기 코루틴
    /// Y Scale 0 → 1 증가 → 유지 → 1 → 0 감소
    /// </summary>
    private IEnumerator PourRoutine()
    {
        float elapsed = 0f;

        // 🔺 단계 1: 따르기 시작 (점점 길어짐)
        while (elapsed < pourDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pourDuration;
            // y 스케일 0 → 1로 Lerp
            wineStreamRenderer.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, t), 1);
            yield return null;
        }

        // 🔺 단계 2: 유지 (길어진 상태로 1초 유지)
        yield return new WaitForSeconds(1f);

        elapsed = 0f;

        // 🔺 단계 3: 따르기 끝 (점점 줄어듦)
        while (elapsed < pourDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pourDuration;
            // y 스케일 1 → 0로 Lerp
            wineStreamRenderer.transform.localScale = new Vector3(1, Mathf.Lerp(1, 0, t), 1);
            yield return null;
        }

        // 🔺 단계 4: 완전히 줄어들면 오브젝트 비활성화
        wineStreamRenderer.gameObject.SetActive(false);
    }
}

