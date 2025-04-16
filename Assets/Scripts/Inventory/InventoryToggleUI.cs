using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InventoryToggleUI : MonoBehaviour
{
    public RectTransform inventoryPanel;  // �κ��丮 �г� RectTransform
    public float slideDuration = 2f;    // �����̵� �ð�
    public Vector2 hiddenPosition = new Vector2(200f, 0); // ������ �ٱ� ��ġ
    public Vector2 visiblePosition = new Vector2(0, 0);   // ȭ�� ���� ��ġ

    private bool isOpen = false;

    private void Start()
    {
        inventoryPanel.anchoredPosition = hiddenPosition; // ���� ��ġ�� ����
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            inventoryPanel.DOAnchorPos(visiblePosition, slideDuration).SetEase(Ease.OutCubic);
        }
        else
        {
            inventoryPanel.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.InCubic);
        }
    }
}
