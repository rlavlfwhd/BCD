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
    }

    IEnumerator ShowItemText()
    {
        if (textPanel != null)
            textPanel.SetActive(true); // �ؽ�Ʈ �г� �ѱ�

        if (itemText != null)
            itemText.text = $"{itemName} ȹ��!";

        yield return new WaitForSeconds(displayTime); // 2�� ���

        if (textPanel != null)
            textPanel.SetActive(false); // �ؽ�Ʈ �г� ����

        gameObject.SetActive(false); // �������� �ڱ� �ڽ� ��Ȱ��ȭ
    }
}
