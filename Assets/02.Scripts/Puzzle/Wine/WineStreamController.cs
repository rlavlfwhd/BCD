using System.Collections;
using UnityEngine;

public class WineStreamController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer wineStreamRenderer; // 🍷 와인 선 SpriteRenderer
    [SerializeField] private float pourDuration = 1.5f;         // 🍷 내려오는 시간
    [SerializeField] private float targetScaleY = 1f;           // 🍷 최종 길이 (1배)

    private void Awake()
    {
        // 시작할 때 Scale.y = 0 (안 보이는 상태)
        wineStreamRenderer.transform.localScale = new Vector3(1, 0, 1);
        wineStreamRenderer.enabled = false; // 처음엔 꺼놓음
    }

    public void StartPouring()
    {
        wineStreamRenderer.enabled = true; // 렌더러 활성화
        StartCoroutine(ExtendStream());
    }

    private IEnumerator ExtendStream()
    {
        float elapsed = 0f;
        Vector3 startScale = new Vector3(1, 0, 1);
        Vector3 endScale = new Vector3(1, targetScaleY, 1);

        while (elapsed < pourDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / pourDuration;
            wineStreamRenderer.transform.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }

        // 최종 값 고정
        wineStreamRenderer.transform.localScale = endScale;
    }

    public void StopPouring()
    {
        // 원래 상태로 복구 (선 사라짐)
        wineStreamRenderer.enabled = false;
        wineStreamRenderer.transform.localScale = new Vector3(1, 0, 1);
    }
}
