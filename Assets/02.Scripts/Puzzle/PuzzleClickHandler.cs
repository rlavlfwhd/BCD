using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleClickHandler : MonoBehaviour
{
    [SerializeField] private LayerMask blockerMask;
    [SerializeField] private LayerMask puzzleLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
            {
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D blockHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, blockerMask);
            RaycastHit2D[] puzzleHits = Physics2D.RaycastAll(mousePos, Vector2.zero, Mathf.Infinity, puzzleLayer);

            RaycastHit2D targetHit = default;
            bool hasTarget = false;
            int highestOrder = int.MinValue;

            foreach (var hit in puzzleHits)
            {
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sortingOrder > highestOrder)
                {
                    highestOrder = sr.sortingOrder;
                    targetHit = hit;
                    hasTarget = true;
                }
            }

            // 클릭 막기
            if (blockHit.collider != null)
            {
                SpriteRenderer blockSR = blockHit.collider.GetComponent<SpriteRenderer>();
                if (blockSR != null && blockSR.sortingOrder >= highestOrder)
                {
                    Debug.Log("Block 오브젝트에 의해 클릭 차단됨");
                    return;
                }
            }

            // 퍼즐 클릭 실행
            if (hasTarget)
            {
                IClickablePuzzle puzzle = targetHit.collider.GetComponentInParent<IClickablePuzzle>();
                if (puzzle != null)
                {
                    Debug.Log("IClickablePuzzle 클릭 호출: " + targetHit.collider.gameObject.name);
                    puzzle.OnClickPuzzle();
                }
            }
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var graphic = result.gameObject.GetComponent<UnityEngine.UI.Graphic>();
            if (graphic != null && graphic.raycastTarget)
                return true;
        }

        return false;
    }
}