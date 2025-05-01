using UnityEngine;

/// <summary>
/// 각 꽃에 붙는 클릭 감지 스크립트
/// </summary>
public class FlowerClickHandler : MonoBehaviour
{
    private FlowerController flowerController;
    private FlowerPuzzleController flowerPuzzleController;

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

