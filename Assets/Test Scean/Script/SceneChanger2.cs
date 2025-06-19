using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger2 : MonoBehaviour
{
    private void OnMouseDown()
    {

        StartCoroutine(FadeManager.Instance.FadeToChoiceScene("PFlowerScene2"));
    }
}
