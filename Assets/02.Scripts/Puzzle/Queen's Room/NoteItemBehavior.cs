using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteItemBehavior : MonoBehaviour
{
    public GameObject noteViewRoot;      // ���� �� ������Ʈ(��ģ ���� �г�)
    public GameObject darkFilterImage;   // ��ũ���� �� �ɼ�
    public GameObject noteObject;        // ���� ������Ʈ

    // ���� ��� ���� ����

    public void SetActiveNotePanel(bool active)
    {
        if (darkFilterImage != null)
            darkFilterImage.SetActive(active);

        if (noteObject != null)
            noteObject.SetActive(active);
    }
}
