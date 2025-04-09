using UnityEngine;
using TMPro;
using System.Collections;

public class ItemClick : MonoBehaviour
{
    public GameObject textPanel;               // �ؽ�Ʈ UI ��ü (��Ȱ�� ���·� ����)
    public TextMeshProUGUI itemText;           // �ؽ�Ʈ ������Ʈ
    public string itemName = "Ŀư";            // ������ �̸�
    public float displayTime = 2f;             // �ؽ�Ʈ ���̴� �ð�

    private void OnMouseDown()
    {
        StartCoroutine(ShowItemText());
        Destroy(gameObject); // Ŭ���� ������ ������Ʈ ����
    }

    IEnumerator ShowItemText()
    {
        if (textPanel != null) textPanel.SetActive(true);
        if (itemText != null) itemText.text = $"{itemName} ȹ��!";

        yield return new WaitForSeconds(displayTime);

        if (textPanel != null) textPanel.SetActive(false);
    }
}
