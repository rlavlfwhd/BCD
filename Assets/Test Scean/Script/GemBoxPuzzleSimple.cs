using UnityEngine;

/// <summary>
/// 보석함 퍼즐 매니저 (슬롯 퍼즐 + 클릭 스왑 가능)
/// </summary>
public class GemBoxPuzzleSimple : MonoBehaviour
{
    [Header("슬롯 오브젝트 4개를 순서 상관 없이 드래그해서 연결")]
    public GemSlotSimple[] slots;

    [Header("퍼즐에 사용할 보석 오브젝트 4개를 연결 (Inspector에서 드래그)")]
    public GemSimple[] gems;

    [Header("닫힌 보석함 / 열린 보석함")]
    public GameObject boxClosed;
    public GameObject boxOpen;

    private Vector3[] originalGemPositions;

    // 현재 슬롯용으로 선택된 보석
    private GemSimple selectedGem = null;

    // ✅ 보석끼리 스왑용 선택 상태
    private GemSimple firstSwapGem = null;

    private void Start()
    {
        if (boxOpen != null) boxOpen.SetActive(false);
        if (boxClosed != null) boxClosed.SetActive(true);

        originalGemPositions = new Vector3[gems.Length];
        for (int i = 0; i < gems.Length; i++)
        {
            originalGemPositions[i] = gems[i].transform.position;
        }

        foreach (var slot in slots)
        {
            slot.currentGemName = "";
        }
    }

    /// <summary>
    /// 보석을 클릭하면 호출됨 (슬롯 배치용)
    /// </summary>
    public void SelectGem(GemSimple gem)
    {
        selectedGem = gem;
        Debug.Log($"[GemBoxPuzzleSimple] Selected Gem: {selectedGem.gemName}");
    }

    /// <summary>
    /// 슬롯을 클릭하면 호출됨 → 선택된 보석을 해당 슬롯으로 이동
    /// </summary>
    public void PlaceGemInSlot(GemSlotSimple slot)
    {
        if (selectedGem == null)
        {
            Debug.Log("[GemBoxPuzzleSimple] 슬롯을 누르기 전에 보석을 먼저 선택하세요.");
            return;
        }

        if (!string.IsNullOrEmpty(slot.currentGemName))
        {
            Debug.Log("[GemBoxPuzzleSimple] 이 슬롯엔 이미 보석이 있습니다.");
            selectedGem = null;
            return;
        }

        selectedGem.HideGem();
        selectedGem.ShowAtSlot(slot.transform);

        slot.currentGemName = selectedGem.gemName;
        selectedGem = null;

        if (AreAllSlotsFilled())
        {
            CheckAllSlotsOrder();
        }
    }

    private bool AreAllSlotsFilled()
    {
        foreach (var slot in slots)
        {
            if (string.IsNullOrEmpty(slot.currentGemName))
                return false;
        }
        return true;
    }

    private void CheckAllSlotsOrder()
    {
        bool allCorrect = true;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentGemName != slots[i].correctGemName)
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            OpenBox();
        }
        else
        {
            Debug.Log("[GemBoxPuzzleSimple] 잘못된 순서! 퍼즐을 초기화합니다.");
            ResetPuzzle();
        }
    }

    private void OpenBox()
    {
        Debug.Log("[GemBoxPuzzleSimple] 퍼즐 성공! 보석함이 열렸습니다.");

        if (boxClosed != null) boxClosed.SetActive(false);
        if (boxOpen != null) boxOpen.SetActive(true);

        for (int i = 0; i < gems.Length; i++)
        {
            if (gems[i] != null)
                gems[i].gameObject.SetActive(false);
        }
    }

    private void ResetPuzzle()
    {
        foreach (var slot in slots)
        {
            slot.currentGemName = "";
        }

        for (int i = 0; i < gems.Length; i++)
        {
            var gem = gems[i];
            gem.transform.position = originalGemPositions[i];
            gem.gameObject.SetActive(true);
        }

        if (boxOpen != null) boxOpen.SetActive(false);
        if (boxClosed != null) boxClosed.SetActive(true);

        selectedGem = null;
        firstSwapGem = null;

        Debug.Log("[GemBoxPuzzleSimple] 퍼즐이 초기화되어 모든 보석이 원래 위치로 돌아갔습니다.");
    }

    // ✅ 추가: 보석끼리 위치 교환용 클릭 함수
    public void SelectGemForSwap(GemSimple gem)
    {
        if (firstSwapGem == null)
        {
            firstSwapGem = gem;
            Debug.Log($"[스왑 선택] 첫 번째 보석 선택됨: {gem.gemName}");
        }
        else if (firstSwapGem == gem)
        {
            Debug.Log("[스왑 선택] 같은 보석 두 번 클릭 → 선택 취소");
            firstSwapGem = null;
        }
        else
        {
            Debug.Log($"[스왑 실행] {firstSwapGem.gemName} ↔ {gem.gemName}");
            SwapGemPositions(firstSwapGem, gem);
            firstSwapGem = null;
        }
    }

    // ✅ 보석끼리 위치 스왑
    private void SwapGemPositions(GemSimple gemA, GemSimple gemB)
    {
        Vector3 temp = gemA.transform.position;
        gemA.transform.position = gemB.transform.position;
        gemB.transform.position = temp;

        Debug.Log("✅ 보석 위치가 서로 스왑되었습니다!");
    }
}
