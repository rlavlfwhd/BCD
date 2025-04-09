using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropTarget
{
    void OnItemDropped(Item item);
}