using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using static UnityEngine.Rendering.VolumeComponent;

public class UIItemClickHandler : MonoBehaviour
{
    public Inventory inventory;
    public GameObject invent;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invent.SetActive(!invent.activeSelf);
        }


        if (Input.GetMouseButtonDown(0))
        {
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
                }
            }
        }
    }
}