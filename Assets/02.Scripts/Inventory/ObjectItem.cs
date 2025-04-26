using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectItem : MonoBehaviour, IObjectItem
{    
    public Item item;
       
    public Item ClickItem()
    {
        return this.item;
    }
}