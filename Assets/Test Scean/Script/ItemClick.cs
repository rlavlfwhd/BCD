using UnityEngine;
using TMPro;
using System.Collections;

public class ItemClick : MonoBehaviour
{
    public GameObject textPanel;               // 텍스트 UI 전체 (비활성 상태로 시작)
    public TextMeshProUGUI itemText;           // 텍스트 컴포넌트
    public string itemName = "커튼";            // 아이템 이름
    public float displayTime = 2f;             // 텍스트 보이는 시간

    private void OnMouseDown()
    {
        StartCoroutine(ShowItemText());
    }

    IEnumerator ShowItemText()
    {
        if (textPanel != null)
            textPanel.SetActive(true); // 텍스트 패널 켜기

        if (itemText != null)
            itemText.text = $"{itemName} 획득!";

        yield return new WaitForSeconds(displayTime); // 2초 대기

        if (textPanel != null)
            textPanel.SetActive(false); // 텍스트 패널 끄기

        gameObject.SetActive(false); // 마지막에 자기 자신 비활성화
    }
}
