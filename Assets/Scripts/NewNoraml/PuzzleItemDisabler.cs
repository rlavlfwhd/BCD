using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItemDisabler : MonoBehaviour
{
    void Start()
    {
        Debug.Log(" PuzzleItemDisabler ���۵�");

        var acquiredIDs = SceneDataManager.Instance.Data.acquiredItemIDs;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in allObjects)
        {
            string goName = go.name.Replace("(Clone)", "").Trim();

            if (go.GetComponent<IObjectItem>() == null)
            {
                Debug.Log($" ���õ� (���� ������ �ƴ�): {go.name}");
                continue;
            }

            if (acquiredIDs.Contains(goName))
            {
                Debug.Log($" ȹ���� ������ �� ��Ȱ��ȭ: {go.name}");

                go.SetActive(false);
            }
            else
            {
                Debug.Log($" ��ȹ�� ������ �� Ȱ��ȭ: {go.name}");

                go.SetActive(true);
            }
        }
    }

}
