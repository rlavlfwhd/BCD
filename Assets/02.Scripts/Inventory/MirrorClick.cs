using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorClick : MonoBehaviour
{
    public GameObject mirrorObj;

    private void OnMouseDown()
    {
        if (mirrorObj != null)
        {
            mirrorObj.SetActive(true);
            Debug.Log("🪞 Background(1) 오브젝트가 활성화됨!");
        }
    }
}


