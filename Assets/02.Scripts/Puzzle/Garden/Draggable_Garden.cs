using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable_Garden : DraggableObj2D
{
    // ���� �پ��ִ� Ÿ��(����)�� ����
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

        // ���� ���� �÷��� ���
        if (foundTile != null)
        {
            // ������ �پ� �ִ� ������ �ִٸ� isOn = false�� �ʱ�ȭ
            if (currentTile != null && currentTile != foundTile)
            {
                currentTile.isOn = false;
            }

            // ���� ���� ������ currentTile�� ����, isOn = true
            currentTile = foundTile;
            currentTile.isOn = true;
        }
        else
        {
            // ������ �ƴ� ���� ������ ��
            if (currentTile != null)
            {
                currentTile.isOn = false;
                currentTile = null;
            }
            ReturnToOriginalPosition();
        }
    }
}
