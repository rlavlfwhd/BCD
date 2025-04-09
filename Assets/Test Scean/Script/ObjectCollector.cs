using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectCollector : MonoBehaviour
{
    public GameObject itemPopupObject;      // �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI itemPopupText;           // �ؽ�Ʈ ������Ʈ
    public float popupDuration = 2f;        // �ؽ�Ʈ ǥ�� �ð�

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
                    ShowPopup(itemName + "�� ȹ���߽��ϴ�!");

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
