using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameClickHandler : MonoBehaviour
{
    public GameObject targetObject; // Inspector에서 AA 오브젝트를 드래그해서 연결

    void OnMouseDown()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log("? AA 오브젝트가 활성화되었습니다.");
        }
        else
        {
            Debug.LogWarning("?? targetObject (AA)가 할당되지 않았습니다.");
        }
    }
}