using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIItemClickHandler : MonoBehaviour
{
    public Inventory inventory;
    public GameObject invent;

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
                    //  아이템 획득 처리
                    Item item = objectItem.ClickItem();
                    if (item != null)
                    {
                        Inventory.Instance.AddItem(item);
                        Debug.Log($"아이템 획득: {item.itemName}");

                        SceneDataManager.Instance.Data.acquiredItemIDs.Add(hit.collider.gameObject.name);
                        hit.collider.gameObject.SetActive(false); // 획득 후 비활성화
                        return;
                    }
                }

                //  선택된 아이템을 오브젝트에 사용하는 처리
                if (Inventory.Instance.firstSelectedItem != null)
                {
                    Item selected = Inventory.Instance.firstSelectedItem;

                    if (hit.collider.name == "사용할 오브젝트 이름")
                    {
                        Inventory.Instance.RemoveItemByName(selected.itemName);
                        Inventory.Instance.ClearSelection();
                        Debug.Log($"오브젝트에 {selected.itemName} 사용됨");
                    }
                }
            }
        }
    }
}