using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectCollector : MonoBehaviour
{
    public GameObject itemPopupObject;      // 텍스트 오브젝트
    public TextMeshProUGUI itemPopupText;           // 텍스트 컴포넌트
    public float popupDuration = 2f;        // 텍스트 표시 시간

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Collectible"))
                {
                    string itemName = hit.collider.name;
                    ShowPopup(itemName + "를 획득했습니다!");

                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    void ShowPopup(string message)
    {
        if (itemPopupObject != null && itemPopupText != null)
        {
            itemPopupObject.SetActive(true);
            itemPopupText.text = message;

            CancelInvoke(nameof(HidePopup));
            Invoke(nameof(HidePopup), popupDuration);
        }
    }

    void HidePopup()
    {
        if (itemPopupObject != null)
        {
            itemPopupObject.SetActive(false);
        }
    }
}
