using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanityClickHandler : MonoBehaviour, IClickablePuzzle
{
    [Header("Mirror 관련 부모 오브젝트")]
    public GameObject mirrorRoot;

    public void OnClickPuzzle()
    {
        if (mirrorRoot != null)
        {
            mirrorRoot.SetActive(true);
            Debug.Log(" 거울 부모 오브젝트 활성화됨");
        }
    }
}
