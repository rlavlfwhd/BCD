using UnityEngine;
using TMPro;
using System.Collections;

public class ItemClick : MonoBehaviour
{
    public GameObject textPanel;               // 텍스트 UI 전체
    public TextMeshProUGUI itemText;           // 텍스트 컴포넌트
    public string itemName = "커튼";            // 아이템 이름
    public float displayTime = 2f;             // 텍스트가 보이는 시간

    private void OnMouseDown()
    {
        if (textPanel != null)
        {
            textPanel.SetActive(true);
        }

        if (itemText != null)
        {
            itemText.text = $"{itemName} 획득!";
        }

        // 텍스트 패널 끄는 코루틴은 외부에서 계속 진행되도록 분리
        StartCoroutine(HideTextPanelAfterDelay());

        // 오브젝트는 즉시 사라짐
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
