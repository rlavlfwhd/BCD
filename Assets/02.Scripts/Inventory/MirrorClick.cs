using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorClick : MonoBehaviour
{
    public GameObject mirrorObj;      // 활성화할 Background(1)
    public GameObject currentBackground; // 기존 Background

    private void OnMouseDown()
    {
        if (mirrorObj != null)
        {
            mirrorObj.SetActive(true);
            Debug.Log("🪞 Background(1) 오브젝트가 활성화됨!");

            if (currentBackground != null)
            {
                currentBackground.SetActive(false);
                Debug.Log("🚫 기존 Background가 비활성화됨");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ mirrorObj가 설정되지 않았습니다.");
        }
    }
}
