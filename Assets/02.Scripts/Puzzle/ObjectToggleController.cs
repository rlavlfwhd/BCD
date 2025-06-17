using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectToggleController : MonoBehaviour
{
    public GameObject buttonObject;       // backHam ��ư
    public Button toggleButton;           // ��ư ����
    public GameObject[] targets;          // ����� ������Ʈ��

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleActiveObjects); // ��ư Ŭ�� �̺�Ʈ ���
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

        // ��ư ������Ʈ ��ü�� Ȱ��/��Ȱ�� ó��
        if (buttonObject.activeSelf != isAnyActive)
        {
            buttonObject.SetActive(isAnyActive);
            Debug.Log("��ư ���� �����: " + isAnyActive);
        }
    }

    void ToggleActiveObjects()
    {
        foreach (GameObject obj in targets)
        {
            if (obj.activeSelf)
            {
                obj.SetActive(false);  // ���� �ִ� ������Ʈ�� ��
                Debug.Log(obj.name + " ��Ȱ��ȭ��");
            }
        }
    }
}