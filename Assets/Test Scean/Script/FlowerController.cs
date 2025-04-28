using UnityEngine;

/// <summary>
/// 🌸 FlowerController
/// 나비가 도착하면 꽃잎을 자연스럽게 떨어뜨리는 기능을 담당
/// </summary>
public class FlowerController : MonoBehaviour
{
    [Header("🌸 꽃 설정")]
    [Tooltip("현재 남아 있는 꽃잎 수입니다.")]
    public int currentPetalCount = 5;

    [Tooltip("최소로 남겨야 하는 꽃잎 수입니다.")]
    public int minPetalCount = 1;

    [Header("🌼 떨어지는 꽃잎 프리팹 설정")]
    [Tooltip("떨어질 때 생성할 꽃잎 프리팹입니다.")]
    public GameObject petalPrefab;

    [Tooltip("꽃잎이 생성될 위치 오프셋입니다.")]
    public Vector3 spawnOffset = new Vector3(0, 0.5f, 0);

    [Header("⚡ 꽃잎 낙하 속도 설정")]
    [Tooltip("꽃잎이 떨어질 때 적용할 중력 세기입니다.")]
    public float fallSpeed = 5.0f; // Inspector에서 조정하는 낙하 속도

    /// <summary>
    /// 꽃잎 하나를 떨어뜨린다 (나비가 도착하면 호출)
    /// </summary>
    public void DropPetal()
    {
        Debug.Log("🌸 DropPetal() 호출됨!");

        if (currentPetalCount > minPetalCount)
        {
            currentPetalCount--;
            Debug.Log($"🌸 {gameObject.name} 꽃잎 하나 떨어짐! 남은 꽃잎 수: {currentPetalCount}");

            if (petalPrefab != null)
            {
                GameObject petal = Instantiate(petalPrefab, transform.position + spawnOffset, Quaternion.identity);
                Debug.Log("🌸 꽃잎 프리팹 생성 완료!");

                Rigidbody2D rb = petal.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    // Rigidbody2D 세팅
                    rb.simulated = true;
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = fallSpeed;    // Inspector 설정 낙하 속도 적용
                    rb.drag = 0.3f;                 // ✨ Linear Drag 설정 (공기 저항 부드럽게)
                    rb.angularDrag = 10.0f;          // ✨ Angular Drag 설정 (회전 거의 못 하게)
                    rb.constraints = RigidbodyConstraints2D.None;

                    // 🌟 좌우로 살짝 흔들리는 힘 추가
                    Vector2 randomForce = new Vector2(Random.Range(-0.5f, 0.5f), 0);
                    rb.AddForce(randomForce, ForceMode2D.Impulse);
                }
                else
                {
                    Debug.LogWarning("⚠️ 생성된 꽃잎에 Rigidbody2D가 없습니다!");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ petalPrefab이 비어있습니다. 연결을 확인하세요!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 꽃잎이 최소 갯수라 더 이상 떨어뜨릴 수 없습니다.");
        }
    }
}






