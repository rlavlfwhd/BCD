using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSwapOnClick : MonoBehaviour
{
    public GameObject targetObject; // 위에 있는 오브젝트

    void OnMouseDown()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);  // 위 오브젝트 켜기
            gameObject.SetActive(false);   // 자기 자신 끄기
            Debug.Log("🔄 아래 오브젝트 → 위 오브젝트로 변환됨");
        }
        else
        {
            Debug.LogWarning("⚠️ targetObject가 할당되지 않았습니다.");
        }
    }
}
