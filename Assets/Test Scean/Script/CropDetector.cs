using UnityEngine;

/// <summary>
/// 퍼즐 타일 감지 전용
/// </summary>
public class CropDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlaceSlot"))
        {
            PuzzleTile tile = other.GetComponent<PuzzleTile>();            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlaceSlot"))
        {
            PuzzleTile tile = other.GetComponent<PuzzleTile>();
        }
    }
}
