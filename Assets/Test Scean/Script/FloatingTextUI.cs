using UnityEngine;
using System.Collections;

public class FloatingTextUI : MonoBehaviour
{
    public GameObject textPanel;       // �ؽ�Ʈâ ��ü UI ������Ʈ
    public float displayTime = 2f;     // ǥ�� �ð� (��)

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (textPanel == null)
        {
            textPanel = this.gameObject;  // ���� �� ������ �ڱ� �ڽ� ���
        }
        textPanel.SetActive(false);
    }

    public void ShowPanel()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }
        currentRoutine = StartCoroutine(ShowAndHidePanel());
    }

    private IEnumerator ShowAndHidePanel()
    {
        textPanel.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        textPanel.SetActive(false);
    }
}
