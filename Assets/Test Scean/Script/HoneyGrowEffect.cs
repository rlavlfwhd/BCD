using UnityEngine;

public class HoneyGrowEffect : MonoBehaviour
{
    [Header("꿀 성장 설정")]
    [Tooltip("시작할 때 꿀방울의 크기입니다.")]
    public Vector3 initialScale = new Vector3(0.5f, 0.5f, 0.5f);

    [Tooltip("성장 완료 시 꿀방울의 최종 크기입니다.")]
    public Vector3 targetScale = Vector3.one;

    [Tooltip("크기가 커지는 데 걸리는 시간(초)입니다.")]
    public float growTime = 0.5f;

    private float timer = 0f;

    void Start()
    {
        transform.localScale = initialScale; // 시작할 때 설정된 크기로 시작
    }

    void Update()
    {
        if (timer < growTime)
        {
            timer += Time.deltaTime;
            float t = timer / growTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        }
    }
}

