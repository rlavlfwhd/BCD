using UnityEngine;

/// <summary>
/// 슬롯(빈 구멍) 오브젝트용 스크립트
/// </summary>
public class GemSlotSimple : MonoBehaviour
{
    [Header("이 슬롯에 넣어야 할 보석 이름")]
    public string correctGemName;

    [HideInInspector]
    public string currentGemName = "";

    // 슬롯 위에 보석 모양을 시각적으로 표시하려면, 
    // 슬롯 자식으로 만들어둔 SpriteRenderer를 연결하거나, 없으면 단순히 보석 오브젝트가 해당 위치로 이동하도록만 처리
    [Header("Optional: 슬롯 내부에 보석을 표시할 SpriteRenderer")]
    public SpriteRenderer slotRenderer;

    private GemBoxPuzzleSimple puzzleManager;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<GemBoxPuzzleSimple>();

        // 슬롯 내부 시각 표시를 위해 slotRenderer가 있다면 투명 상태로 만들어 둠
        if (slotRenderer != null)
        {
            slotRenderer.color = new Color(1f, 1f, 1f, 0f); // 완전 투명
        }
    }

    /// <summary>
    /// MonoBehaviour 기본 메서드: 슬롯에 붙은 Collider2D를 마우스로 클릭하면 호출
    /// </summary>
    private void OnMouseDown()
    {
        Debug.Log($"[GemSlotSimple] Slot clicked: {correctGemName}");
        if (puzzleManager != null)
        {
            puzzleManager.PlaceGemInSlot(this);
        }
    }

    /// <summary>
    /// 퍼즐 매니저에서 호출: 실제 보석(GameObject)을 슬롯 위치로 순간이동시키고,
    /// 슬롯 내부 SpriteRenderer가 있으면 보석 이미지를 표시해 줌
    /// </summary>
    public void SetGem(string gemName, Sprite gemSprite, Transform gemTransform)
    {
        currentGemName = gemName;

        // 보석 오브젝트를 슬롯 위치로 순간이동
        gemTransform.position = this.transform.position;

        // 슬롯 내부 시각 표시가 있다면, 해당 스프라이트와 투명도를 켜서 “보석이 끼워진 모양”을 보여줌
        if (slotRenderer != null)
        {
            slotRenderer.sprite = gemSprite;
            slotRenderer.color = Color.white;
        }
    }

    /// <summary>
    /// 이 슬롯이 올바른 보석을 갖고 있는지 확인
    /// </summary>
    public bool IsCorrect()
    {
        return currentGemName == correctGemName;
    }
}
