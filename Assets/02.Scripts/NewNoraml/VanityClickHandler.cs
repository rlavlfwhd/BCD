using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanityClickHandler : MonoBehaviour, IClickablePuzzle
{
    [SerializeField] private GameObject mirrorObject;

    public void OnClickPuzzle()
    {
        if (mirrorObject != null)
        {
            mirrorObject.SetActive(true);
            Debug.Log("Vanity Å¬¸¯ ¡æ Mirror ÆÛÁñ ÄÑÁü");
        }
    }
}
