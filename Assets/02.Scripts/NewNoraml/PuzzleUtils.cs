using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleUtils
{
    public static void DisableAcquiredItemObjects()
    {
        if (SceneDataManager.Instance == null || SceneDataManager.Instance.Data == null)
        {
            Debug.LogWarning("SceneDataManager�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        var acquiredIDs = SceneDataManager.Instance.Data.acquiredItemIDs;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // ��Ȱ��ȭ�� ������Ʈ�� ����

        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<IObjectItem>() == null)
            {
                continue; // IObjectItem�� ���� ������Ʈ�� �ǳʶٱ�
            }

            string name = go.name.Replace("(Clone)", "").Trim();

            if (acquiredIDs.Contains(name))
            {
                go.SetActive(false); // �̹� ȹ���� �������� ��Ȱ��ȭ
                Debug.Log($"[PuzzleUtils] ��Ȱ��ȭ�� ȹ�� ������ ������Ʈ: {name}");
            }
        }
    }
}
