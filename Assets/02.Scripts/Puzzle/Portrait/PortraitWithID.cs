using UnityEngine;

public class PortraitWithID : MonoBehaviour, IClickablePuzzle
{
    public int ID;
    public int CurrentIndex { get; private set; }

    private PortraitSwapperWithSlots manager;

    private void Start()
    {
        manager = FindObjectOfType<PortraitSwapperWithSlots>();
    }

    public void SetIndex(int index)
    {
        CurrentIndex = index;
    }

    public void OnClickPuzzle()
    {
        if (manager != null)
        {
            manager.OnPortraitClicked(this);
        }
    }
}
