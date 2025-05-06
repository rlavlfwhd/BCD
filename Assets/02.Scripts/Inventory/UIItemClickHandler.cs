using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIItemClickHandler : MonoBehaviour
{
    public Inventory inventory;
    public GameObject invent;

    [SerializeField] LayerMask blockerMask;
    [SerializeField] LayerMask clickable2DMask;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI())
            {                                
                return;
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D blockHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, blockerMask);
            RaycastHit2D itemHit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, clickable2DMask);

            if (blockHit.collider != null)
            {
                if (itemHit.collider == null || blockHit.distance < itemHit.distance)
                {
                    Debug.Log("2D ≈¨∏Ø ¬˜¥‹µ ");
                    return;
                }
            }

            if (itemHit.collider != null)
            {
                GameObject target = itemHit.collider.gameObject;

                IObjectItem objectItem = target.GetComponent<IObjectItem>();
                if (objectItem != null)
                {
                    Item item = objectItem.ClickItem();
                    if (item != null)
                    {
                        Inventory.Instance.AddItem(item);
                        Debug.Log($"[2D] æ∆¿Ã≈€ »πµÊ: {item.itemName}");

                        SceneDataManager.Instance.Data.acquiredItemIDs.Add(target.name);
                        target.SetActive(false);
                    }
                }
            }
        }
    }

    private bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var graphic = result.gameObject.GetComponent<UnityEngine.UI.Graphic>();
            if (graphic != null && graphic.raycastTarget)
                return true;
        }

        return false;
    }
}
