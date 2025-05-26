using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameClickHandler : MonoBehaviour
{
    public GameObject targetObject; // Inspector���� AA ������Ʈ�� �巡���ؼ� ����

    void OnMouseDown()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Debug.Log("? AA ������Ʈ�� Ȱ��ȭ�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("?? targetObject (AA)�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}