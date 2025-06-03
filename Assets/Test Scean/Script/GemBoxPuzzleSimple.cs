using UnityEngine;

/// <summary>
/// ������ ���� �Ŵ��� (������)
/// - ���� �ϳ��� Ŭ���ؼ� SelectGem(��)���� ����
/// - ������ Ŭ���ؼ� PlaceGemInSlot(��)���� ���Կ� �̵�
/// - �� �� ������ ��� ä�� �� �� ���� ���� �˻�
/// - ������ ������ �ڽ� ����(���� ��Ȱ��ȭ)
/// - ������ Ʋ���� �ʱ�ȭ
/// </summary>
public class GemBoxPuzzleSimple : MonoBehaviour
{
    [Header("���� ������Ʈ 4���� ���� ��� ���� �巡���ؼ� ����")]
    public GemSlotSimple[] slots;

    [Header("���� ����� ���� ������Ʈ 4���� ���� (Inspector���� �巡��)")]
    public GemSimple[] gems;

    [Header("���� ������ / ���� ������")]
    public GameObject boxClosed;
    public GameObject boxOpen;

    // ���� ���� ��ġ �����
    private Vector3[] originalGemPositions;

    // ���� ���õ� ����
    private GemSimple selectedGem = null;

    private void Start()
    {
        // ���� �ڽ��� ��Ȱ��, ���� �ڽ��� Ȱ��
        if (boxOpen != null) boxOpen.SetActive(false);
        if (boxClosed != null) boxClosed.SetActive(true);

        // gems �迭�� ��� �ִ� ��� ������ original positions ���
        originalGemPositions = new Vector3[gems.Length];
        for (int i = 0; i < gems.Length; i++)
        {
            originalGemPositions[i] = gems[i].transform.position;
        }

        // ���Ե��� currentGemName �ʱ�ȭ
        foreach (var slot in slots)
        {
            slot.currentGemName = "";
        }
    }

    /// <summary>
    /// GemSimple���� ȣ��: Ŭ���� ������ ����
    /// </summary>
    public void SelectGem(GemSimple gem)
    {
        selectedGem = gem;
        Debug.Log($"[GemBoxPuzzleSimple] Selected Gem: {selectedGem.gemName}");
    }

    /// <summary>
    /// GemSlotSimple���� ȣ��: ���õ� ������ �� ���Կ� �Ⱦ� ��
    /// </summary>
    public void PlaceGemInSlot(GemSlotSimple slot)
    {
        // 1) ������ ���� �������� �ʾ����� �ȳ� �� ����
        if (selectedGem == null)
        {
            Debug.Log("[GemBoxPuzzleSimple] ������ ������ ���� ������ ���� �����ϼ���.");
            return;
        }

        // 2) �̹� �� ���Կ� ������ ���� �ִ��� Ȯ��
        if (!string.IsNullOrEmpty(slot.currentGemName))
        {
            Debug.Log("[GemBoxPuzzleSimple] �� ���Կ� �̹� ������ �ֽ��ϴ�.");
            selectedGem = null;
            return;
        }

        // 3) ���� ��ġ�� ������ ���� �̵���Ű�� ��, ��� ����
        selectedGem.HideGem();

        // 4) ���� ��ġ�� �̵��� ��, �ٽ� Ȱ��ȭ (���� Z �� ����)
        selectedGem.ShowAtSlot(slot.transform);
        Debug.Log($"[GemBoxPuzzleSimple] {slot.correctGemName} ���Կ� {selectedGem.gemName} ���� ��ġ��.");

        // 5) ���� �����Ϳ� ���� �̸� ���
        slot.currentGemName = selectedGem.gemName;

        // 6) ���õ� ���� ���� �ʱ�ȭ
        selectedGem = null;

        // 7) �� �� ������ ��� ä�������� Ȯ��, ä�������� �� ���� ���� �˻�
        if (AreAllSlotsFilled())
        {
            CheckAllSlotsOrder();
        }
    }

    /// <summary>
    /// �� �� ������ ��� ��� ���� ������(= ������ ��� ä��������) Ȯ��
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
    /// ���� �������(currentGemName) ��ü�� ��� correctGemName�� ��ġ�ϴ��� �˻�
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
            // ��� ������ �����̸� �ڽ� ����
            OpenBox();
        }
        else
        {
            // �ϳ��� Ʋ���� ���� �ʱ�ȭ
            Debug.Log("[GemBoxPuzzleSimple] �߸��� ����! ������ �ʱ�ȭ�մϴ�.");
            ResetPuzzle();
        }
    }

    /// <summary>
    /// ���� �ϼ� �� ȣ��: ���� �ڽ��� ����� ���� �ڽ��� ���� �ָ�,
    /// ���ÿ� ��� ������ ��Ȱ��ȭ�Ͽ� �� ���̰� ����ϴ�.
    /// </summary>
    private void OpenBox()
    {
        Debug.Log("[GemBoxPuzzleSimple] ���� ����! �������� ���Ƚ��ϴ�.");

        // 1) ������ ���� �ݱ� ó��
        if (boxClosed != null) boxClosed.SetActive(false);
        if (boxOpen != null) boxOpen.SetActive(true);

        // 2) ���� ���� ��� ������ ��Ȱ��ȭ�ؼ� ȭ�鿡�� ������� ��
        for (int i = 0; i < gems.Length; i++)
        {
            if (gems[i] != null)
                gems[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// �߸��� ������ ������ �Ⱦ��� �� ȣ��: ������ ù ���·� �ǵ���
    /// </summary>
    private void ResetPuzzle()
    {
        // 1) ���� ������ �ʱ�ȭ
        foreach (var slot in slots)
        {
            slot.currentGemName = "";
        }

        // 2) ��� ������ ���� ��ġ�� �̵� �� Ȱ��ȭ
        for (int i = 0; i < gems.Length; i++)
        {
            var gem = gems[i];
            gem.transform.position = originalGemPositions[i];
            gem.gameObject.SetActive(true);
        }

        // 3) �������� ���� ���·� ����
        if (boxOpen != null) boxOpen.SetActive(false);
        if (boxClosed != null) boxClosed.SetActive(true);

        // 4) ���õ� ���� �ʱ�ȭ
        selectedGem = null;

        Debug.Log("[GemBoxPuzzleSimple] ������ �ʱ�ȭ�Ǿ� ��� ������ ���� ��ġ�� ���ư����ϴ�.");
    }
}
