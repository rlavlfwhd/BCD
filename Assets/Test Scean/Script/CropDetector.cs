using UnityEngine;

public class CropDetector : MonoBehaviour
{
    // 퍼즐 타일과 충돌이 시작되었을 때
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"🚨 [OnTriggerEnter] 감지된 오브젝트: {other.name}, 태그: {other.tag}");

        PuzzleTile tile = other.GetComponent<PuzzleTile>();
        if (tile != null)
        {
            Debug.Log($"✅ 퍼즐 타일 감지 성공: {other.name}");
            tile.isOn = true;
            tile.UpdateVisual();
        }
        else
        {
            Debug.Log($"❌ PuzzleTile 컴포넌트가 없음: {other.name}");
        }
    }

    // 충돌이 유지되는 동안 매 프레임 호출됨
    private void OnTriggerStay(Collider other)
    {
        //Debug.Log($"🌀 [OnTriggerStay] 감지된 오브젝트: {other.name}, 태그: {other.tag}");
    }

    // 퍼즐 타일과 충돌이 끝났을 때
    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"⛔ [OnTriggerExit] 벗어난 오브젝트: {other.name}, 태그: {other.tag}");

        PuzzleTile tile = other.GetComponent<PuzzleTile>();
        if (tile != null)
        {
            Debug.Log($"🔄 퍼즐 타일 상태 해제: {other.name}");
            tile.isOn = false;
            tile.UpdateVisual();
        }
    }
}



