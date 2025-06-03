using UnityEngine;

public class GemSimple : MonoBehaviour
{
    [Header("���� �̸� (\"Spring\", \"Summer\", \"Fall\", \"Winter\")")]
    public string gemName;

    private GemBoxPuzzleSimple puzzleManager;
    private SpriteRenderer sr;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<GemBoxPuzzleSimple>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log($"[GemSimple] OnMouseDown fired on {this.gameObject.name}");
        if (puzzleManager != null)
        {
            puzzleManager.SelectGem(this);
        }
    }

    /// <summary>
    /// �������� �̵��ϱ� ���� ��� ���� �� ���
    /// </summary>
    public void HideGem()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// ���� ��ġ�� �̵� �� �ٽ� ���̵���. 
    /// ������ X/Y�� ����ϰ�, Z�� ���� �״�� �����մϴ�.
    /// </summary>
    public void ShowAtSlot(Transform slotTransform)
    {
        // ���� ������ Z ���� ����
        float originalZ = this.transform.position.z;

        // ������ X/Y�� �������� Z�� ���� �� ����
        Vector3 newPos = new Vector3(
            slotTransform.position.x,
            slotTransform.position.y,
            originalZ
        );

        this.transform.position = newPos;
        this.gameObject.SetActive(true);
    }
}
