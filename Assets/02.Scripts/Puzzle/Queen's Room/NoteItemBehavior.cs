using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteItemBehavior : MonoBehaviour
{
    public GameObject noteViewRoot;           // 쪽지 뷰 오브젝트(펼친 쪽지 패널)
    public GameObject darkFilterImage;        // 다크필터 등 옵션

    private bool isActive = false;

    public void OnClickNote()
    {
        isActive = !isActive;
        noteViewRoot.SetActive(isActive);

        if (darkFilterImage != null)
            darkFilterImage.SetActive(isActive);
    }
}
