using UnityEngine;

public class PetalMovement : MonoBehaviour
{
    [Header("흔들림 설정")]
    public float swayAmount = 1.5f;  // 🌟 흔들림 크기를 확실히 키움
    public float swaySpeed = 2.0f;   // 흔들림 속도

    private Rigidbody2D rb;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Drag 값도 조금 줄여준다
            rb.drag = 0.2f;
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        timer += Time.fixedDeltaTime;

        // 🌟 sin 곡선을 직접 x축 속도에 적용
        float swayVelocity = Mathf.Sin(timer * swaySpeed) * swayAmount;

        // 🌟 y축 속도는 그대로 유지하고, x축만 살짝 왔다갔다
        Vector2 newVelocity = new Vector2(swayVelocity, rb.velocity.y);
        rb.velocity = newVelocity;
    }
}

