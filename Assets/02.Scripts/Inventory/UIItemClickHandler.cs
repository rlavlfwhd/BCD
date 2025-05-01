using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIItemClickHandler : MonoBehaviour
{
    public Inventory inventory;
    public GameObject invent;

    [SerializeField] LayerMask blockerMask;      // ClickBlocker    
    [SerializeField] LayerMask clickable3DMask;  // Clickable3D

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;            
            Ray ray3D = Camera.main.ScreenPointToRay(mousePos);

            // === 3D √≥∏Æ ===
            bool hasHit3D = Physics.Raycast(ray3D, out RaycastHit hit3D, Mathf.Infinity, clickable3DMask);
            bool hasBlock3D = Physics.Raycast(ray3D, out RaycastHit block3D, Mathf.Infinity, blockerMask);

            if (hasBlock3D)
            {
                if (!hasHit3D || block3D.distance < hit3D.distance)
                {
                    Debug.Log("3D ≈¨∏Ø ¬˜¥‹µ ");
                    return;
                }
            }

            if (hasHit3D)
            {
                GameObject target = hit3D.collider.gameObject;

                // ∆€¡Ò ø¿∫Í¡ß∆Æ¥¬ π´Ω√
                if (target.GetComponent<WindowPuzzle>() != null ||
                    target.GetComponent<SecretPath>() != null)
                {
                    return;
                }

                IObjectItem objectItem = target.GetComponent<IObjectItem>();
                if (objectItem != null)
                {
                    Item item = objectItem.ClickItem();
                    if (item != null)
                    {
                        Inventory.Instance.AddItem(item);
                        Debug.Log($"[3D] æ∆¿Ã≈€ »πµÊ: {item.itemName}");

                        SceneDataManager.Instance.Data.acquiredItemIDs.Add(target.name);
                        target.SetActive(false);
                    }
                }
            }
        }
    }
}
