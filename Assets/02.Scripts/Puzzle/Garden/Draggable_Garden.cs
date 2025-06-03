using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable_Garden : DraggableObj2D
{
    public override void OnDragEnd()
    {
        float detectionRadius = 1.0f;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("PlaceSlot"))
            {
                Vector3 slotPosition = hit.transform.position;
                slotPosition.z = transform.position.z;
                StartCoroutine(SmoothMove(transform.position, slotPosition, 0.2f));

                PuzzleTile tile = hit.GetComponent<PuzzleTile>();
                if (tile != null)
                {
                    tile.isOn = true;
                }

                return;
            }
        }

        ReturnToOriginalPosition();
    }
}
