using UnityEngine;

/// <summary>
/// 보석함 퍼즐 매니저 (수정됨)
/// - 보석 하나를 클릭해서 SelectGem(…)으로 선택
/// - 슬롯을 클릭해서 PlaceGemInSlot(…)으로 슬롯에 이동
/// - 네 개 슬롯을 모두 채운 뒤 한 번에 순서 검사
/// - 순서가 맞으면 박스 열기(보석 비활성화)
/// - 순서가 틀리면 초기화
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

    // 보석 원래 위치 저장용
    private Vector3[] originalGemPositions;

    // 현재 선택된 보석
    private GemSimple selectedGem = null;

    private void Start()
    {
        // 열린 박스는 비활성, 닫힌 박스는 활성
        if (boxOpen != null) boxOpen.SetActive(false);
        if (boxClosed != null) boxClosed.SetActive(true);

        // gems 배열에 들어 있는 모든 보석의 original positions 기록
        originalGemPositions = new Vector3[gems.Length];
        for (int i = 0; i < gems.Length; i++)
        {
            originalGemPositions[i] = gems[i].transform.position;
        }

        // 슬롯들의 currentGemName 초기화
        foreach (var slot in slots)
        {
            slot.currentGemName = "";
        }
    }

    /// <summary>
    /// GemSimple에서 호출: 클릭한 보석을 선택
    /// </summary>
    public void SelectGem(GemSimple gem)
    {
        selectedGem = gem;
        Debug.Log($"[GemBoxPuzzleSimple] Selected Gem: {selectedGem.gemName}");
    }

    /// <summary>
    /// GemSlotSimple에서 호출: 선택된 보석을 이 슬롯에 꽂아 줌
    /// </summary>
    public void PlaceGemInSlot(GemSlotSimple slot)
    {
        // 1) 보석을 먼저 선택하지 않았으면 안내 후 종료
        if (selectedGem == null)
        {
            Debug.Log("[GemBoxPuzzleSimple] 슬롯을 누르기 전에 보석을 먼저 선택하세요.");
            return;
        }

        // 2) 이미 이 슬롯에 보석이 꽂혀 있는지 확인
        if (!string.IsNullOrEmpty(slot.currentGemName))
        {
            Debug.Log("[GemBoxPuzzleSimple] 이 슬롯엔 이미 보석이 있습니다.");
            selectedGem = null;
            return;
        }

        // 3) 슬롯 위치로 보석을 순간 이동시키기 전, 잠시 숨김
        selectedGem.HideGem();

        // 4) 슬롯 위치로 이동한 뒤, 다시 활성화 (원래 Z 값 유지)
        selectedGem.ShowAtSlot(slot.transform);
        Debug.Log($"[GemBoxPuzzleSimple] {slot.correctGemName} 슬롯에 {selectedGem.gemName} 보석 배치됨.");

        // 5) 슬롯 데이터에 보석 이름 기록
        slot.currentGemName = selectedGem.gemName;

        // 6) 선택된 보석 정보 초기화
        selectedGem = null;

        // 7) 네 개 슬롯이 모두 채워졌는지 확인, 채워졌으면 한 번에 순서 검사
        if (AreAllSlotsFilled())
        {
            CheckAllSlotsOrder();
        }
    }

    /// <summary>
    /// 네 개 슬롯이 모두 비어 있지 않은지(= 보석이 모두 채워졌는지) 확인
    /// </summary>
    private bool AreAllSlotsFilled()
    {
        foreach (var slot in slots)
        {
            if (string.IsNullOrEmpty(slot.currentGemName))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 슬롯 순서대로(currentGemName) 전체가 모두 correctGemName과 일치하는지 검사
    /// </summary>
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
            // 모든 순서가 정답이면 박스 열기
            OpenBox();
        }
        else
        {
            // 하나라도 틀리면 퍼즐 초기화
            Debug.Log("[GemBoxPuzzleSimple] 잘못된 순서! 퍼즐을 초기화합니다.");
            ResetPuzzle();
        }
    }

    /// <summary>
    /// 퍼즐 완성 시 호출: 닫힌 박스를 숨기고 열린 박스를 보여 주며,
    /// 동시에 모든 보석을 비활성화하여 안 보이게 만듭니다.
    /// </summary>
    private void OpenBox()
    {
        Debug.Log("[GemBoxPuzzleSimple] 퍼즐 성공! 보석함이 열렸습니다.");

        // 1) 보석함 열고 닫기 처리
        if (boxClosed != null) boxClosed.SetActive(false);
        if (boxOpen != null) boxOpen.SetActive(true);

        // 2) 퍼즐에 사용된 모든 보석을 비활성화해서 화면에서 사라지게 함
        for (int i = 0; i < gems.Length; i++)
        {
            if (gems[i] != null)
                gems[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 잘못된 순서로 보석을 꽂았을 때 호출: 퍼즐을 첫 상태로 되돌림
    /// </summary>
    private void ResetPuzzle()
    {
        // 1) 슬롯 데이터 초기화
        foreach (var slot in slots)
        {
            slot.currentGemName = "";
        }

        // 2) 모든 보석을 원래 위치로 이동 및 활성화
        for (int i = 0; i < gems.Length; i++)
        {
            var gem = gems[i];
            gem.transform.position = originalGemPositions[i];
            gem.gameObject.SetActive(true);
        }

        // 3) 보석함은 닫힌 상태로 유지
        if (boxOpen != null) boxOpen.SetActive(false);
        if (boxClosed != null) boxClosed.SetActive(true);

        // 4) 선택된 보석 초기화
        selectedGem = null;

        Debug.Log("[GemBoxPuzzleSimple] 퍼즐이 초기화되어 모든 보석이 원래 위치로 돌아갔습니다.");
    }
}
