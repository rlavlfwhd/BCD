
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<Item> items = new List<Item>();

    public Item selectedItem;
    public Item firstSelectedItem;
    public Item secondSelectedItem;

    [SerializeField] private Transform slotParent;
    [SerializeField] private Slot[] slots;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        FreshSlot();
    }

    public void AddItem(Item _item)
    {
        if (!items.Contains(_item))
        {
            items.Add(_item);
            FreshSlot();
        }
    }

    public void RemoveItem(Item _item)
    {
        if (items.Contains(_item))
        {
            items.Remove(_item);
            FreshSlot();
        }
    }

    public void RemoveItemByName(string itemName)
    {
        var item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            items.Remove(item);
            Debug.Log($"�κ��丮���� ���ŵ�: {itemName}");
            FreshSlot();
        }
    }

    public void SelectItem(Item item)
    {
        if (firstSelectedItem == item || secondSelectedItem == item)
        {
            Debug.Log($"���� ������: {item.itemName}");
            ClearSelection();
            return;
        }

        if (firstSelectedItem == null)
        {
            firstSelectedItem = item;
            Debug.Log($"ù ��° ����: {item.itemName}");
        }
        else if (secondSelectedItem == null)
        {
            secondSelectedItem = item;
            Debug.Log($"�� ��° ����: {item.itemName}");
            CombineItems(firstSelectedItem, secondSelectedItem);
            ClearSelection();
        }
    }

    public void ClearSelection()
    {
        firstSelectedItem = null;
        secondSelectedItem = null;
    }

    public void FreshSlot()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
                slots[i].SetItem(items[i]);
            else
                slots[i].ClearSlot();
        }
    }

    public void RefreshSlotReference()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        FreshSlot();
    }

    public bool HasCreatedRope(string ropeID)
    {
        return SceneDataManager.Instance.Data.createdRopes.Contains(ropeID);
    }

    public void RegisterCreatedRope(string ropeID)
    {
        SceneDataManager.Instance.Data.createdRopes.Add(ropeID);
    }

    public void CombineItems(Item item1, Item item2)
    {
        string name1 = item1.itemName;
        string name2 = item2.itemName;

        if ((IsRopeMaterial(name1) && IsRopeMaterial(name2)) && name1 != name2)
        {
            if (!HasCreatedRope("Rope"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope");
            else
                Debug.Log("�̹� Rope ������. �ٽ� ���� �� ����.");
            return;
        }

        if (IsUpgradeCombo(name1, name2, "Rope"))
        {
            if (!HasCreatedRope("Rope2"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope2");
            else
                Debug.Log("�̹� Rope2 ������. �ٽ� ���� �� ����.");
            return;
        }

        if (IsUpgradeCombo(name1, name2, "Rope2"))
        {
            if (!HasCreatedRope("Rope3"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope3");
            else
                Debug.Log("�̹� Rope3 ������. �ٽ� ���� �� ����.");
            return;
        }

        if (IsUpgradeCombo(name1, name2, "Rope3"))
        {
            if (!HasCreatedRope("Rope4"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope4");
            else
                Debug.Log("�̹� Rope4 ������. �ٽ� ���� �� ����.");
            return;
        }

        Debug.Log("���� ����: ���� ����ġ");
    }

    private bool IsRopeMaterial(string name)
    {
        return name == "CurtainL" || name == "CurtainR" ||
               name == "BedCurtainL" || name == "BedCurtainR" ||
               name == "Blanket";
    }

    private bool IsUpgradeCombo(string a, string b, string ropeLevel)
    {
        return (a == ropeLevel && IsRopeMaterial(b)) ||
               (b == ropeLevel && IsRopeMaterial(a));
    }

    private void TryCreateCombinedItem(string[] ingredients, string resultItemPath)
    {
        foreach (string name in ingredients)
        {
            var found = items.Find(i => i.itemName == name);
            if (found != null)
            {
                items.Remove(found);
                Debug.Log($"���ŵ� ������: {found.itemName}");
            }
        }

        Item newItem = Resources.Load<Item>(resultItemPath);
        if (newItem != null)
        {
            AddItem(newItem);
            RegisterCreatedRope(newItem.itemName);
            Debug.Log($"���� ����: {newItem.itemName}");
        }
        else
        {
            Debug.LogError($"���� ������ �ε� ����: {resultItemPath}");
        }

        FreshSlot();
    }
}
