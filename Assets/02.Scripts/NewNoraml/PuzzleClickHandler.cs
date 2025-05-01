using System.Collections;
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
                Debug.Log("[PuzzleClickHandler] UI 위 클릭 감지됨 → 무시");
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool hasBlock = Physics.Raycast(ray, out RaycastHit blockHit, Mathf.Infinity, blockerMask);

            // 퍼즐 레이어 검사
            bool hasHitPuzzle = Physics.Raycast(ray, out RaycastHit puzzleHit, Mathf.Infinity, puzzleLayer);

            if (hasBlock)
            {
                if (!hasHitPuzzle || blockHit.distance < puzzleHit.distance)
                {
                    Debug.Log("Block 레이어에 의해 퍼즐 클릭 차단됨");
                    return;
                }
            }

            if (hasHitPuzzle)
            {
                Debug.Log($" 퍼즐 충돌: {puzzleHit.collider.name}");

                IClickablePuzzle puzzle = puzzleHit.collider.GetComponentInParent<IClickablePuzzle>();
                if (puzzle != null)
                {
                    puzzle.OnClickPuzzle();
                    Debug.Log(" 퍼즐 인터페이스 실행됨");
                }
                else
                {
                    Debug.LogWarning(" IClickablePuzzle 인터페이스 못 찾음");
                }
            }
            else
            {
                Debug.Log(" 퍼즐 레이어에 아무것도 안 맞음");
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