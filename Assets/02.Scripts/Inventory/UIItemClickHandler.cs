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
                GameObject target = hit.collider.gameObject;

                // 퍼즐 오브젝트는 무시 (자체 스크립트 처리)
                if (target.GetComponent<WindowPuzzle>() != null ||
                    target.GetComponent<SecretPath>() != null)
                {
                    return;
                }

                IObjectItem objectItem = hit.collider.GetComponent<IObjectItem>();

                if (objectItem != null)
                {
                    //  아이템 획득 처리
                    Item item = objectItem.ClickItem();
                    if (item != null)
                    {
                        Inventory.Instance.AddItem(item);
                        Debug.Log($"아이템 획득: {item.itemName}");

                        SceneDataManager.Instance.Data.acquiredItemIDs.Add(target.name);
                        target.SetActive(false); // 획득 후 비활성화
                    }
                }                
            }
        }
    }
}