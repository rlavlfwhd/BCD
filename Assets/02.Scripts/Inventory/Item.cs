using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
}