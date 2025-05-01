using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUtils : MonoBehaviour
{
    public static bool IsPointerOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            // UI 레이어만 필터링하거나, RaycastTarget 있는 것만 판별
            if (result.gameObject.GetComponent<Graphic>() != null && result.gameObject.GetComponent<Graphic>().raycastTarget)
            {
                return true;
            }
        }

        return false;
    }
}
