using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemDisabler : MonoBehaviour
{
    void Start()
    {
        Debug.Log(" PuzzleItemDisabler Ω√¿€µ ");

        var acquiredIDs = SceneDataManager.Instance.Data.acquiredItemIDs;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in allObjects)
        {
            string goName = go.name.Replace("(Clone)", "").Trim();

            if (go.GetComponent<IObjectItem>() == null)
            {
                Debug.Log($" π´Ω√µ  (∆€¡Ò æ∆¿Ã≈€ æ∆¥‘): {go.name}");
                continue;
            }

            if (acquiredIDs.Contains(goName))
            {
                Debug.Log($" »πµÊ«— æ∆¿Ã≈€ °Ê ∫Ò»∞º∫»≠: {go.name}");

                go.SetActive(false);
            }
            else
            {
                Debug.Log($" πÃ»πµÊ æ∆¿Ã≈€ °Ê »∞º∫»≠: {go.name}");

                go.SetActive(true);
            }
        }
    }

}
