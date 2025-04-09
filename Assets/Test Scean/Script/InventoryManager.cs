using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private List<string> items = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� �̵��ص� �����ǵ���
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log(itemName + "��(��) �κ��丮�� �߰��Ǿ����ϴ�.");
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }
}
