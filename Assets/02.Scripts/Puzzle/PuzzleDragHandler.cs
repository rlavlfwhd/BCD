using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleDragHandler : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private LayerMask dragLayer;

    private DraggableObj2D draggingObject = null;
    private Vector3 dragOffset;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI()) return;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D blockHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, blockLayer);
            RaycastHit2D[] dragHits = Physics2D.RaycastAll(mousePos, Vector2.zero, Mathf.Infinity, dragLayer);

            RaycastHit2D targetHit = default;
            bool hasTarget = false;
            int highestOrder = int.MinValue;

            foreach (var hit in dragHits)
            {
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sortingOrder > highestOrder)
                {
                    highestOrder = sr.sortingOrder;
                    targetHit = hit;
                    hasTarget = true;
                }
            }

            if (blockHit.collider != null)
            {
                SpriteRenderer blockSR = blockHit.collider.GetComponent<SpriteRenderer>();
                if (blockSR != null && blockSR.sortingOrder >= highestOrder)
                {
                    Debug.Log("Block 오브젝트에 의해 드래그 차단됨");
                    return;
                }
            }

            if (hasTarget)
            {
                draggingObject = targetHit.collider.GetComponent<DraggableObj2D>();
                if (draggingObject != null)
                {
                    dragOffset = draggingObject.transform.position - (Vector3)mousePos;
                    draggingObject.OnDragStart();
                }
            }
        }

        if (draggingObject != null)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePoint.z = 0;
                draggingObject.OnDragMove(mousePoint + dragOffset);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                draggingObject.OnDragEnd();
                draggingObject = null;
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
