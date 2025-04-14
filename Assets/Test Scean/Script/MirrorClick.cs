using UnityEngine;

public class MirrorClick : MonoBehaviour
{
    public GameObject layer2;
    public GameObject layer0;

    private bool isLayer2Active = false;

    void OnMouseDown()
    {
        if (!isLayer2Active)
        {
            // 0Layer ¡æ 2Layer ÀüÈ¯
            if (layer2 != null)
                layer2.SetActive(true);
            if (layer0 != null)
                layer0.SetActive(false);
        }
        else
        {
            // 2Layer ¡æ 0Layer º¹±Í
            if (layer2 != null)
                layer2.SetActive(false);
            if (layer0 != null)
                layer0.SetActive(true);
        }

        isLayer2Active = !isLayer2Active;
    }
}
