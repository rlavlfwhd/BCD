using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorClick : MonoBehaviour, IClickablePuzzle
{
    public GameObject mirrorObj;
    public void OnClickPuzzle()
    {
        if (mirrorObj != null)
        {
            mirrorObj.SetActive(true);
        }
    }
}
