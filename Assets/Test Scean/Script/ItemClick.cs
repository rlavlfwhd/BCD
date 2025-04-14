using UnityEngine;
using TMPro;
using System.Collections;

public class ItemClick : MonoBehaviour
{
    public GameObject textPanel;               // �ؽ�Ʈ UI ��ü
    public TextMeshProUGUI itemText;           // �ؽ�Ʈ ������Ʈ
    public string itemName = "Ŀư";            // ������ �̸�
    public float displayTime = 2f;             // �ؽ�Ʈ�� ���̴� �ð�

    private void OnMouseDown()
    {
        if (textPanel != null)
        {
            textPanel.SetActive(true);
        }

        if (itemText != null)
        {
            itemText.text = $"{itemName} ȹ��!";
        }

        // �ؽ�Ʈ �г� ���� �ڷ�ƾ�� �ܺο��� ��� ����ǵ��� �и�
        StartCoroutine(HideTextPanelAfterDelay());

        // ������Ʈ�� ��� �����
        gameObject.SetActive(false);
    }

    IEnumerator HideTextPanelAfterDelay()
    {
        yield return new WaitForSeconds(displayTime);

        if (textPanel != null)
        {
            textPanel.SetActive(false);
        }
    }
}
