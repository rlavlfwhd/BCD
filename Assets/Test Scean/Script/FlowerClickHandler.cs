using UnityEngine;

/// <summary>
/// 꽃 오브젝트에 붙여서 클릭을 감지하는 스크립트
/// </summary>
public class FlowerClickHandler : MonoBehaviour
{
    private FlowerController flowerController; // 자기 자신의 꽃 컨트롤러
    private FlowerPuzzleController flowerPuzzleController; // 퍼즐 매니저

    private void Start()
    {
        flowerController = GetComponent<FlowerController>();
        flowerPuzzleController = FindObjectOfType<FlowerPuzzleController>();
    }

    private void OnMouseDown()
    {
        if (flowerPuzzleController != null && flowerController != null)
        {
            Debug.Log($"🌸 클릭한 꽃: {gameObject.name}");
            flowerPuzzleController.OnFlowerClicked(flowerController);
        }
    }
}
