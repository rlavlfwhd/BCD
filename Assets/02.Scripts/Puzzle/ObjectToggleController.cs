using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToggleController : MonoBehaviour
{
    public GameObject buttonObject;       // backHam 버튼
    public Button toggleButton;           // 버튼 연결
    public GameObject[] targets;          // 토글할 오브젝트들

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleActiveObjects); // 버튼 클릭 이벤트 등록
    }

    void Update()
    {
        bool isAnyActive = false;

        foreach (GameObject obj in targets)
        {
            if (obj.activeSelf)
            {
                isAnyActive = true;
                break;
            }
        }

        // 버튼 오브젝트 자체를 활성/비활성 처리
        if (buttonObject.activeSelf != isAnyActive)
        {
            buttonObject.SetActive(isAnyActive);
            Debug.Log("버튼 상태 변경됨: " + isAnyActive);
        }
    }

    void ToggleActiveObjects()
    {
        foreach (GameObject obj in targets)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);  // 켜져 있는 오브젝트만 끔
                Debug.Log(obj.name + " 비활성화됨");
            }
        }
    }
}