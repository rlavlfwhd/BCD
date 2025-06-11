using UnityEngine;

/// <summary>
/// 초상화를 클릭해서 슬롯으로 이동시키는 간단한 로직
/// </summary>
public class PortraitSlotMover : MonoBehaviour
{
    // 현재 선택된 Portrait
    private GameObject selectedPortrait = null;

    // 그림, 슬롯 구분용 레이어 설정
    [SerializeField] private LayerMask portraitLayer;
    [SerializeField] private LayerMask slotLayer;

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 1. Portrait 클릭
            RaycastHit2D portraitHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, portraitLayer);
            if (portraitHit.collider != null)
            {
                selectedPortrait = portraitHit.collider.gameObject;
                Debug.Log($"✅ 그림 선택됨: {selectedPortrait.name}");
                return;
            }

            // 2. Slot 클릭
            RaycastHit2D slotHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, slotLayer);
            if (slotHit.collider != null && selectedPortrait != null)
            {
                // 선택된 그림을 슬롯 위치로 이동
                selectedPortrait.transform.position = slotHit.collider.transform.position;
                Debug.Log($"➡ {selectedPortrait.name} 이동됨 → {slotHit.collider.name}");

                // 선택 해제
                selectedPortrait = null;
                return;
            }

            // 3. 아무것도 안 눌렀을 때
            selectedPortrait = null;
        }
    }
}

