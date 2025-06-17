using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanityClickHandler : MonoBehaviour, IClickablePuzzle
{
    [Header("Mirror ���� �θ� ������Ʈ")]
    public GameObject mirrorRoot;

    public void OnClickPuzzle()
    {
        if (mirrorRoot != null)
        {
            mirrorRoot.SetActive(true);
            Debug.Log(" �ſ� �θ� ������Ʈ Ȱ��ȭ��");
        }
    }
}
