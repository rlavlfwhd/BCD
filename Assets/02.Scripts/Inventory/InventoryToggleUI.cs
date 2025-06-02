using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class InventoryToggleUI : MonoBehaviour
{
    public GameObject inventoryPanelObj;
    public RectTransform inventoryPanel;  // 인벤토리 패널 RectTransform
    public float slideDuration = 2f;    // 슬라이드 시간
    public Vector2 hiddenPosition = new Vector2(200f, 0); // 오른쪽 바깥 위치
    public Vector2 visiblePosition = new Vector2(0, 0);   // 화면 안쪽 위치

    private bool isOpen = false;

    private void Start()
    {
        inventoryPanel.anchoredPosition = hiddenPosition; // 시작 위치는 숨김
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
            inventoryPanel.anchoredPosition = hiddenPosition;  // 메인씬일 때 위치 초기화
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
