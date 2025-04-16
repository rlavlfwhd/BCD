using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public List<Item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;



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
        
    public void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AddItem(Item _item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(_item);
            FreshSlot();
        }
        else
        {
            print("슬롯이 가득 차 있습니다.");
        }
    }

    public void RefreshSlotReference()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        FreshSlot();
    }

    private HashSet<string> createdRopes = new HashSet<string>();

    public void CombineItems(Item item1, Item item2)
    {
        string name1 = item1.itemName;
        string name2 = item2.itemName;

        // 커튼/이불끼리 → Rope
        if ((IsRopeMaterial(name1) && IsRopeMaterial(name2)) && name1 != name2)
        {
            if (!createdRopes.Contains("Rope"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope");
            else
                Debug.Log(" 이미 Rope 생성됨. 다시 만들 수 없음.");
            return;
        }

        // Rope + 커튼/이불 → Rope2
        if (IsUpgradeCombo(name1, name2, "Rope"))
        {
            if (!createdRopes.Contains("Rope2"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope2");
            else
                Debug.Log(" 이미 Rope2 생성됨. 다시 만들 수 없음.");
            return;
        }

        // Rope2 + 커튼/이불 → Rope3
        if (IsUpgradeCombo(name1, name2, "Rope2"))
        {
            if (!createdRopes.Contains("Rope3"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope3");
            else
                Debug.Log(" 이미 Rope3 생성됨. 다시 만들 수 없음.");
            return;
        }

        // Rope3 + 커튼/이불 → Rope4
        if (IsUpgradeCombo(name1, name2, "Rope3"))
        {
            if (!createdRopes.Contains("Rope4"))
                TryCreateCombinedItem(new string[] { name1, name2 }, "Items/Rope4");
            else
                Debug.Log(" 이미 Rope4 생성됨. 다시 만들 수 없음.");
            return;
        }

        Debug.Log(" 조합 실패: 조합 조건 불일치");
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
                Debug.Log($" 제거된 아이템: {found.itemName}");
            }
        }

        Item newItem = Resources.Load<Item>(resultItemPath);
        if (newItem != null)
        {
            AddItem(newItem);
            createdRopes.Add(newItem.itemName); //  생성된 로프 등록
            Debug.Log($" 조합 성공: {newItem.itemName}");
        }
        else
        {
            Debug.LogError($" 조합 아이템 로드 실패: {resultItemPath}");
        }

        FreshSlot();
    }

}
