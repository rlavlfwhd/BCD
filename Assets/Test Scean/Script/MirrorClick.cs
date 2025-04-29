using UnityEngine;

public class MirrorClick : MonoBehaviour
{    
    public GameObject mirrorObj;


    void OnMouseDown()
    {
        mirrorObj.SetActive(true);
    }
}
