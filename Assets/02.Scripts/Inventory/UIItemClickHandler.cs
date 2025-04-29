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

                // ���� ������Ʈ�� ���� (��ü ��ũ��Ʈ ó��)
                if (target.GetComponent<WindowPuzzle>() != null ||
                    target.GetComponent<SecretPath>() != null)
                {
                    return;
                }

                IObjectItem objectItem = hit.collider.GetComponent<IObjectItem>();

                if (objectItem != null)
                {
                    //  ������ ȹ�� ó��
                    Item item = objectItem.ClickItem();
                    if (item != null)
                    {
                        Inventory.Instance.AddItem(item);
                        Debug.Log($"������ ȹ��: {item.itemName}");

                        SceneDataManager.Instance.Data.acquiredItemIDs.Add(target.name);
                        target.SetActive(false); // ȹ�� �� ��Ȱ��ȭ
                    }
                }                
            }
        }
    }
}