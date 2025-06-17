using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class InventoryToggleUI : MonoBehaviour
{
    public GameObject inventoryPanelObj;
    public RectTransform inventoryPanel;  // �κ��丮 �г� RectTransform
    public CanvasGroup inventoryCanvasGroup;
    public float slideDuration = 0.5f;    // �����̵� �ð�
    public Vector2 hiddenPosition = new Vector2(200f, 0); // ������ �ٱ� ��ġ
    public Vector2 visiblePosition = new Vector2(0, 0);   // ȭ�� ���� ��ġ

    private bool isOpen = false;
    private bool isSliding = false;

    private void Start()
    {
        inventoryPanel.anchoredPosition = hiddenPosition; // ���� ��ġ�� ����

        if (inventoryCanvasGroup != null)
        {
            inventoryCanvasGroup.interactable = false;
            inventoryCanvasGroup.blocksRaycasts = false;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            inventoryPanelObj.SetActive(false);
            inventoryPanel.anchoredPosition = hiddenPosition;  // ���ξ��� �� ��ġ �ʱ�ȭ
            isOpen = false;

            if (inventoryCanvasGroup != null)
            {
                inventoryCanvasGroup.interactable = false;
                inventoryCanvasGroup.blocksRaycasts = false;
            }
        }
        else
        {
            inventoryPanelObj.SetActive(true);
        }
    }

    public void ToggleInventory()
    {
        if (isSliding) return;

        isOpen = !isOpen;

        isSliding = true;

        inventoryPanel.DOKill();

        if (inventoryCanvasGroup != null)
        {
            inventoryCanvasGroup.interactable = false;
            inventoryCanvasGroup.blocksRaycasts = false;
        }

        Debug.Log("Tween Start: isOpen=" + isOpen + "  duration=" + slideDuration);

        if (isOpen)
        {
            inventoryPanel.DOAnchorPos(visiblePosition, slideDuration).SetEase(Ease.Linear)
                .OnComplete(() => {
                    if (inventoryCanvasGroup != null)
                    {
                        inventoryCanvasGroup.interactable = true;
                        inventoryCanvasGroup.blocksRaycasts = true;
                    }
                    isSliding = false;
                    Debug.Log("Tween Complete: Open");
                });
        }
        else
        {
            inventoryPanel.DOAnchorPos(hiddenPosition, slideDuration).SetEase(Ease.Linear)
                .OnComplete(() => {
                    if (inventoryCanvasGroup != null)
                    {
                        inventoryCanvasGroup.interactable = false;
                        inventoryCanvasGroup.blocksRaycasts = false;
                    }
                    isSliding = false;
                    Debug.Log("Tween Complete: Close");
                });
        }
    }
}