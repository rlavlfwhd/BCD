using UnityEngine;
using System.Collections;

public class FloatingTextUI : MonoBehaviour
{
    public GameObject textPanel;       // 텍스트창 전체 UI 오브젝트
    public float displayTime = 2f;     // 표시 시간 (초)
    public Vector3 offset = new Vector3(0, 2f, 0);  // 오브젝트 위에 표시되도록

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (textPanel == null)
        {
            Debug.LogWarning("textPanel이 연결되지 않았습니다.");
        }
        else
        {
            // textPanel이 자기 자신이면 비활성화하지 마!
            if (textPanel != this.gameObject)
            {
                textPanel.SetActive(false);
            }
        }
    }

    public void ShowPanel(Transform target)
    {
        if (textPanel == null) return;

        // 텍스트 위치를 target 위로 옮기기
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
