using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelBoxClickHandler : MonoBehaviour, IClickablePuzzle
{

    public GameObject jewelRoot;

    public void OnClickPuzzle()
    {
        if (jewelRoot != null)
        {
            jewelRoot.SetActive(true);            
        }
    }
}
