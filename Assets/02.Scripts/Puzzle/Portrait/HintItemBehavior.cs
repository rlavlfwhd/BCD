using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintItemBehavior : MonoBehaviour
{
    public GameObject hintViewRoot;      // ���� �� ������Ʈ(��ģ ���� �г�)
    public GameObject darkFilterImage;   // ��ũ���� �� �ɼ�
    public GameObject hintObject;        // ���� ������Ʈ

    // ���� ��� ���� ����

    public void SetActiveNotePanel(bool active)
    {
        if (darkFilterImage != null)
            darkFilterImage.SetActive(active);

        if (hintObject != null)
            hintObject.SetActive(active);
    }
}
