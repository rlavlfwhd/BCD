using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class InventoryToggleUI : MonoBehaviour
{
    public GameObject inventoryPanelObj;
    public RectTransform inventoryPanel;  // �κ��丮 �г� RectTransform
    public float slideDuration = 2f;    // �����̵� �ð�
    public Vector2 hiddenPosition = new Vector2(200f, 0); // ������ �ٱ� ��ġ
    public Vector2 visiblePosition = new Vector2(0, 0);   // ȭ�� ���� ��ġ

    private bool isOpen = false;

    private void Start()
    {
        inventoryPanel.anchoredPosition = hiddenPosition; // ���� ��ġ�� ����
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
        }
        else
        {
            inventoryPanelObj.SetActive(true);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            return;
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;

        inventoryPanel.DOKill();

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
