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
                    //  ������ ȹ�� ó��
                    Item item = objectItem.ClickItem();
                    if (item != null)
                    {
                        Inventory.Instance.AddItem(item);
                        Debug.Log($"������ ȹ��: {item.itemName}");

                        SceneDataManager.Instance.Data.acquiredItemIDs.Add(hit.collider.gameObject.name);
                        hit.collider.gameObject.SetActive(false); // ȹ�� �� ��Ȱ��ȭ
                        return;
                    }
                }

                //  ���õ� �������� ������Ʈ�� ����ϴ� ó��
                if (Inventory.Instance.firstSelectedItem != null)
                {
                    Item selected = Inventory.Instance.firstSelectedItem;

                    if (hit.collider.name == "����� ������Ʈ �̸�")
                    {
                        Inventory.Instance.RemoveItemByName(selected.itemName);
                        Inventory.Instance.ClearSelection();
                        Debug.Log($"������Ʈ�� {selected.itemName} ����");
                    }
                }
            }
        }
    }
}