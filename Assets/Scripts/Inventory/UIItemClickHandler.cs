using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static UnityEngine.Rendering.VolumeComponent;

public class UIItemClickHandler : MonoBehaviour
{
    public Inventory inventory;
    public GameObject invent;

    void Start()
    {
        Inventory.Instance.RefreshSlotReference();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                IObjectItem objectItem = hit.collider.GetComponent<IObjectItem>();
                if (objectItem != null)
                {
                    Item item = objectItem.ClickItem();
                    print($"{item.itemName}");
                    inventory.AddItem(item);

                    Destroy(hit.collider.gameObject); // æ∆¿Ã≈€ »πµÊ »ƒ ¡¶∞≈
                    return;
                }
            }
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                IObjectItem clickInterface = result.gameObject.GetComponent<IObjectItem>();

                if (clickInterface != null)
                {
                    Item item = clickInterface.ClickItem();
                    print($"{item.itemName}");
                    inventory.AddItem(item);

                    Destroy(result.gameObject);
                }
            }
        }
    }
}