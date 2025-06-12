using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable_Garden : DraggableObj2D
{
    // 현재 붙어있는 타일(슬롯)을 추적
    private PuzzleTile currentTile = null;

    public override void OnDragEnd()
    {
        float detectionRadius = 1.0f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        PuzzleTile foundTile = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("PlaceSlot"))
            {
                foundTile = hit.GetComponent<PuzzleTile>();
                if (foundTile != null)
                {
                    Vector3 slotPosition = hit.transform.position;
                    slotPosition.z = transform.position.z;
                    StartCoroutine(SmoothMove(transform.position, slotPosition, 0.2f));
                }
                break;
            }
        }

        // 슬롯 위에 올려진 경우
        if (foundTile != null)
        {
            // 이전에 붙어 있던 슬롯이 있다면 isOn = false로 초기화
            if (currentTile != null && currentTile != foundTile)
            {
                currentTile.isOn = false;
            }

            // 새로 붙인 슬롯을 currentTile로 저장, isOn = true
            currentTile = foundTile;
            currentTile.isOn = true;
        }
        else
        {
            // 슬롯이 아닌 곳에 놓았을 때
            if (currentTile != null)
            {
                currentTile.isOn = false;
                currentTile = null;
            }
            ReturnToOriginalPosition();
        }
    }
}
