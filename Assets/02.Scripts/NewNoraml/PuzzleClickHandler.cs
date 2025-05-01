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
                Debug.Log("[PuzzleClickHandler] UI �� Ŭ�� ������ �� ����");
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            bool hasBlock = Physics.Raycast(ray, out RaycastHit blockHit, Mathf.Infinity, blockerMask);

            // ���� ���̾� �˻�
            bool hasHitPuzzle = Physics.Raycast(ray, out RaycastHit puzzleHit, Mathf.Infinity, puzzleLayer);

            if (hasBlock)
            {
                if (!hasHitPuzzle || blockHit.distance < puzzleHit.distance)
                {
                    Debug.Log("Block ���̾ ���� ���� Ŭ�� ���ܵ�");
                    return;
                }
            }

            if (hasHitPuzzle)
            {
                Debug.Log($" ���� �浹: {puzzleHit.collider.name}");

                IClickablePuzzle puzzle = puzzleHit.collider.GetComponentInParent<IClickablePuzzle>();
                if (puzzle != null)
                {
                    puzzle.OnClickPuzzle();
                    Debug.Log(" ���� �������̽� �����");
                }
                else
                {
                    Debug.LogWarning(" IClickablePuzzle �������̽� �� ã��");
                }
            }
            else
            {
                Debug.Log(" ���� ���̾ �ƹ��͵� �� ����");
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