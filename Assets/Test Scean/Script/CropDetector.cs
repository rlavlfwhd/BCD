using UnityEngine;

/// <summary>
/// 퍼즐 타일 감지 전용
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
                tile.isOn = false;
                tile.UpdateVisual();
            }
        }
    }
}
