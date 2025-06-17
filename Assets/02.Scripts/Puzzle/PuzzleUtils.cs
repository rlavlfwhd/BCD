using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleUtils
{
    public static void DisableAcquiredItemObjects()
    {
        if (SceneDataManager.Instance == null || SceneDataManager.Instance.Data == null)
        {
            Debug.LogWarning("SceneDataManager가 초기화되지 않았습니다.");
            return;
        }

        var acquiredIDs = SceneDataManager.Instance.Data.acquiredItemIDs;
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // 비활성화된 오브젝트도 포함

        foreach (GameObject go in allObjects)
        {
            if (go.GetComponent<IObjectItem>() == null)
            {
                continue; // IObjectItem이 없는 오브젝트는 건너뛰기
            }

            string name = go.name.Replace("(Clone)", "").Trim();

            if (acquiredIDs.Contains(name))
            {
                go.SetActive(false); // 이미 획득한 아이템은 비활성화
                Debug.Log($"[PuzzleUtils] 비활성화된 획득 아이템 오브젝트: {name}");
            }
        }
    }
}
