using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeHintItemBehavior : MonoBehaviour
{
    public GameObject fakeHintViewRoot;      // 쪽지 뷰 오브젝트(펼친 쪽지 패널)
    public GameObject darkFilterImage;   // 다크필터 등 옵션
    public GameObject fakeHintObject;        // 쪽지 오브젝트

    // 내부 토글 변수 삭제

    public void SetActiveNotePanel(bool active)
    {
        if (darkFilterImage != null)
            darkFilterImage.SetActive(active);

        if (fakeHintObject != null)
            fakeHintObject.SetActive(active);
    }
}
