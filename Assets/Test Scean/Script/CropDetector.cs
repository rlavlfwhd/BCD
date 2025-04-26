using UnityEngine;

/// <summary>
/// 퍼즐 타일 위에 올라가면 isOn을 켜고,
/// 내려가면 isOn을 끄는 단순 감지 오브젝트.
/// 본체(작물)에는 직접 연결하지 않는다.
/// </summary>
public class CropDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlaceSlot"))
        {
            PuzzleTile tile = other.GetComponent<PuzzleTile>();
            if (tile != null)
            {
                Debug.Log($"🟢 감지 성공: {tile.name}");
                tile.isOn = true;
                tile.UpdateVisual();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlaceSlot"))
        {
            PuzzleTile tile = other.GetComponent<PuzzleTile>();
            if (tile != null)
            {
                Debug.Log($"🔴 감지 해제: {tile.name}");
                tile.isOn = false;
                tile.UpdateVisual();
            }
        }
    }
}