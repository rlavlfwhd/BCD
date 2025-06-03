using UnityEngine;

/// <summary>
/// ����(�� ����) ������Ʈ�� ��ũ��Ʈ
/// </summary>
public class GemSlotSimple : MonoBehaviour
{
    [Header("�� ���Կ� �־�� �� ���� �̸�")]
    public string correctGemName;

    [HideInInspector]
    public string currentGemName = "";

    // ���� ���� ���� ����� �ð������� ǥ���Ϸ���, 
    // ���� �ڽ����� ������ SpriteRenderer�� �����ϰų�, ������ �ܼ��� ���� ������Ʈ�� �ش� ��ġ�� �̵��ϵ��ϸ� ó��
    [Header("Optional: ���� ���ο� ������ ǥ���� SpriteRenderer")]
    public SpriteRenderer slotRenderer;

    private GemBoxPuzzleSimple puzzleManager;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<GemBoxPuzzleSimple>();

        // ���� ���� �ð� ǥ�ø� ���� slotRenderer�� �ִٸ� ���� ���·� ����� ��
        if (slotRenderer != null)
        {
            slotRenderer.color = new Color(1f, 1f, 1f, 0f); // ���� ����
        }
    }

    /// <summary>
    /// MonoBehaviour �⺻ �޼���: ���Կ� ���� Collider2D�� ���콺�� Ŭ���ϸ� ȣ��
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
    /// ���� �Ŵ������� ȣ��: ���� ����(GameObject)�� ���� ��ġ�� �����̵���Ű��,
    /// ���� ���� SpriteRenderer�� ������ ���� �̹����� ǥ���� ��
    /// </summary>
    public void SetGem(string gemName, Sprite gemSprite, Transform gemTransform)
    {
        currentGemName = gemName;

        // ���� ������Ʈ�� ���� ��ġ�� �����̵�
        gemTransform.position = this.transform.position;

        // ���� ���� �ð� ǥ�ð� �ִٸ�, �ش� ��������Ʈ�� ������ �Ѽ� �������� ������ ��硱�� ������
        if (slotRenderer != null)
        {
            slotRenderer.sprite = gemSprite;
            slotRenderer.color = Color.white;
        }
    }

    /// <summary>
    /// �� ������ �ùٸ� ������ ���� �ִ��� Ȯ��
    /// </summary>
    public bool IsCorrect()
    {
        return currentGemName == correctGemName;
    }
}
