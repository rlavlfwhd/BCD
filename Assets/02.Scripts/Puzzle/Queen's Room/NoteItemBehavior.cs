using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteItemBehavior : MonoBehaviour
{
    public GameObject noteViewRoot;           // ���� �� ������Ʈ(��ģ ���� �г�)
    public GameObject darkFilterImage;        // ��ũ���� �� �ɼ�

    private bool isActive = false;

    public void OnClickNote()
    {
        isActive = !isActive;
        noteViewRoot.SetActive(isActive);

        if (darkFilterImage != null)
            darkFilterImage.SetActive(isActive);
    }
}
