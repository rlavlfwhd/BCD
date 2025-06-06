﻿using UnityEngine;
using System.Collections;

public enum ItemType { Book, Statue } // ✅ 아이템 타입 정의

public class DraggableItem : MonoBehaviour
{
    [Header("이 책의 고유 이름 (예: BlueBook, RedBook 등)")]
    public string bookName;

    [Header("이 오브젝트의 타입")]
    public ItemType itemType = ItemType.Book; // ✅ Inspector에서 타입 설정 가능

    private Vector3 offset;
    private Vector3 originalPosition;
    private bool isLocked = false;

    private BookSlot currentSlot = null;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (isLocked) return;

        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0;
        offset = transform.position - mousePoint;
        offset.z = 0;

        offset = transform.position - mousePoint;

        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }

        Debug.Log(" 드래그 시작 (2D)");
    }

    void OnMouseDrag()
    {
        if (isLocked) return;

        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePoint.z = 0;

        Vector3 targetPos = mousePoint + offset;
        transform.position = targetPos;
    }

    void OnMouseUp()
    {
        if (isLocked) return;

        Debug.Log(" 드래그 종료 (2D)");

        float detectionRadius = 1.0f;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("DropSlot"))
            {
                BookSlot slot = hit.GetComponent<BookSlot>();
                if (slot != null)
                {
                    bool success = slot.TryInsertBook(gameObject);

                    if (success)
                    {
                        Vector3 slotPosition = hit.transform.position;
                        slotPosition.z = transform.position.z;

                        StartCoroutine(SmoothMove(transform.position, slotPosition, 0.2f));

                        currentSlot = slot;
                        isLocked = false;

                        originalPosition = slotPosition;

                        return;
                    }
                }
            }
        }
    }

    public void ReturnToOriginalPosition()
    {
        StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }
    }

    public void ForceRemoveFromSlot()
    {
        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }

        isLocked = false;
        StartCoroutine(SmoothMove(transform.position, originalPosition, 0.2f));
    }

    public void ForceSwapToPosition(Vector3 targetPosition)
    {
        StartCoroutine(SmoothMove(transform.position, targetPosition, 0.2f));
        if (currentSlot != null)
        {
            currentSlot.isOccupied = false;
            currentSlot.isCorrect = false;
            currentSlot = null;
        }
    }

    public Vector3 GetOriginalPosition()
    {
        return originalPosition;
    }

    IEnumerator SmoothMove(Vector3 fromPos, Vector3 toPos, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(fromPos, toPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = toPos;
    }
}