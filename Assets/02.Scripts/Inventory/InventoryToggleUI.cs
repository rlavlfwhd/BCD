using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class InventoryToggleUI : MonoBehaviour
{
    [Header("?? 인벤토리 패널")]
    public GameObject inventoryPanelObj;          // 인벤토리 전체 오브젝트
    public RectTransform inventoryPanel;          // 패널 RectTransform
    public CanvasGroup inventoryCanvasGroup;      // 캔버스 그룹 (선택)

    [Header("?? 슬라이드 설정")]
    public float slideDuration = 0.5f;
    public Vector2 hiddenPosition = new Vector2(200f, 0f);  // 오른쪽 바깥
    public Vector2 visiblePosition = new Vector2(0f, 0f);  // 화면 안쪽

    private bool isOpen = false;
    private bool isSliding = false;

  
    private void Start()
    {
        // 시작은 닫힌 위치
        ForceCloseInstant();
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    // 씬이 로드될 때마다 인벤토리 창을 닫아 둔다
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ForceCloseInstant();
    }

   
    public void ToggleInventory()
    {
        if (isSliding) return;

        isOpen = !isOpen;
        isSliding = true;

        inventoryPanel.DOKill();            // 기존 트윈 중단

        // 입력·Raycast 잠시 비활성
        SetCanvasInteractable(false);

        if (isOpen)
        {
            inventoryPanel.DOAnchorPos(visiblePosition, slideDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    SetCanvasInteractable(true);
                    isSliding = false;
                });
        }
        else
        {
            inventoryPanel.DOAnchorPos(hiddenPosition, slideDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    isSliding = false;
                });
        }
    }

   
    private void ForceCloseInstant()
    {
        if (inventoryPanelObj != null)
            inventoryPanelObj.SetActive(true);   // 패널은 살아 있어야 버튼으로 재오픈 가능

        inventoryPanel.anchoredPosition = hiddenPosition;
        isOpen = false;
        SetCanvasInteractable(false);
    }

    private void SetCanvasInteractable(bool value)
    {
        if (inventoryCanvasGroup != null)
        {
            inventoryCanvasGroup.interactable = value;
            inventoryCanvasGroup.blocksRaycasts = value;
        }
    }
}
