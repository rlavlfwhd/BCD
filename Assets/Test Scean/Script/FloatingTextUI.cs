using UnityEngine;
using System.Collections;

public class FloatingTextUI : MonoBehaviour
{
    public GameObject textPanel;       // �ؽ�Ʈâ ��ü UI ������Ʈ
    public float displayTime = 2f;     // ǥ�� �ð� (��)
    public Vector3 offset = new Vector3(0, 2f, 0);  // ������Ʈ ���� ǥ�õǵ���

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (textPanel == null)
        {
            Debug.LogWarning("textPanel�� ������� �ʾҽ��ϴ�.");
        }
        else
        {
            // textPanel�� �ڱ� �ڽ��̸� ��Ȱ��ȭ���� ��!
            if (textPanel != this.gameObject)
            {
                textPanel.SetActive(false);
            }
        }
    }

    public void ShowPanel(Transform target)
    {
        if (textPanel == null) return;

        // �ؽ�Ʈ ��ġ�� target ���� �ű��
        textPanel.transform.position = target.position + offset;

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
