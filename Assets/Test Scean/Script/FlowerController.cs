using UnityEngine;

public class FlowerController : MonoBehaviour
{
    [Header("🌸 꽃잎 수 설정")]
    public int currentPetalCount = 4;
    public int minPetalCount = 0;

    [Header("🌸 꽃 스프라이트 리스트")]
    public Sprite[] flowerSprites;
    private SpriteRenderer spriteRenderer;

    [Header("🍃 떨어지는 꽃잎 프리팹 설정")]
    public GameObject petalPrefab; // 떨어지는 꽃잎 프리팹
    public Vector3 petalSpawnOffset = new Vector3(0, 0.5f, 0); // 꽃 위쪽 위치에서 생성

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateFlowerSprite();
    }

    /// <summary>
    /// 꽃잎 하나 떨어뜨리기
    /// </summary>
    public void DropPetal()
    {
        if (currentPetalCount > minPetalCount)
        {
            currentPetalCount--;
            Debug.Log($"🌸 꽃잎 하나 떨어짐! 남은 꽃잎 수: {currentPetalCount}");

            UpdateFlowerSprite();
            SpawnFallingPetal(); // 🌟 꽃잎 프리팹 생성
        }
        else
        {
            Debug.Log("🌸 꽃잎이 더 이상 떨어질 수 없습니다.");
        }
    }

    /// <summary>
    /// 꽃 상태에 따라 스프라이트 변경
    /// </summary>
    private void UpdateFlowerSprite()
    {
        if (flowerSprites != null && currentPetalCount >= 0 && currentPetalCount < flowerSprites.Length)
        {
            spriteRenderer.sprite = flowerSprites[currentPetalCount];
        }
        else
        {
            Debug.LogWarning("⚠️ 꽃 스프라이트 변경 실패: 배열 범위를 벗어났습니다.");
        }
    }

    /// <summary>
    /// 떨어지는 꽃잎 프리팹 생성
    /// </summary>
    private void SpawnFallingPetal()
    {
        if (petalPrefab != null)
        {
            Vector3 spawnPos = transform.position + petalSpawnOffset;
            GameObject petal = Instantiate(petalPrefab, spawnPos, Quaternion.identity);

            // Rigidbody2D가 있다면 아래 방향 힘 적용
            Rigidbody2D rb = petal.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0.7f;  // 낙하 속도
                rb.velocity = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-1.5f, -2.5f));
                rb.angularVelocity = Random.Range(-60f, 60f); // 살짝 회전
            }

            // 자동 제거
            Destroy(petal, 3f); // 3초 뒤 제거
        }
    }
}






