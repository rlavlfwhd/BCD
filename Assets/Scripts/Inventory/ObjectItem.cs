using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    [Header("������")]
    public Item item;
    [Header("������ �̹���")]
    public Image itemImage;

    void Start()
    {
        itemImage.sprite = item.itemImage;
    }
    public Item ClickItem()
    {
        return this.item;
    }
}