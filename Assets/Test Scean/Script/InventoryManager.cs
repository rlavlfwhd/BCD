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
            DontDestroyOnLoad(gameObject); // 씬 이동해도 유지되도록
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log(itemName + "이(가) 인벤토리에 추가되었습니다.");
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }
}
