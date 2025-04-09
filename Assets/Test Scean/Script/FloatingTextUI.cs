using UnityEngine;
using System.Collections;

public class FloatingTextUI : MonoBehaviour
{
    public GameObject textPanel;       // 텍스트창 전체 UI 오브젝트
    public float displayTime = 2f;     // 표시 시간 (초)

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (textPanel == null)
        {
            textPanel = this.gameObject;  // 연결 안 됐으면 자기 자신 사용
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
