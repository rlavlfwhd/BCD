using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeHintItemBehavior : MonoBehaviour
{
    public GameObject fakeHintViewRoot;      // ���� �� ������Ʈ(��ģ ���� �г�)
    public GameObject darkFilterImage;   // ��ũ���� �� �ɼ�
    public GameObject fakeHintObject;        // ���� ������Ʈ

    // ���� ��� ���� ����

    public void SetActiveNotePanel(bool active)
    {
        if (darkFilterImage != null)
            darkFilterImage.SetActive(active);

        if (fakeHintObject != null)
            fakeHintObject.SetActive(active);
    }
}
