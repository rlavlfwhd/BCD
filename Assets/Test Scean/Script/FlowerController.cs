using UnityEngine;

/// <summary>
/// 개별 꽃을 관리하는 스크립트
/// 꽃잎 개수 조정
/// </summary>
public class FlowerController : MonoBehaviour
{
    [Header("🌸 꽃 기본 설정")]
    public int currentPetalCount = 4; // 현재 꽃잎 수
    public int minPetalCount = 0;      // 꽃잎 최소 개수

    /// <summary>
    /// 꽃잎을 하나 떨어뜨림
    /// </summary>
    public void DropPetal()
    {
        if (currentPetalCount > minPetalCount)
        {
            currentPetalCount--;
            Debug.Log($"🌸 {gameObject.name} 꽃잎 떨어짐! 남은 꽃잎: {currentPetalCount}");
        }
        else
        {
            Debug.Log($"🌸 {gameObject.name} 꽃잎이 더 이상 떨어질 수 없음.");
        }
    }
}
