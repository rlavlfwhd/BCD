using UnityEngine;

public class BookShelfClickHandler : MonoBehaviour
{
    public GameObject BookShelfChild;

    void OnMouseDown()
    {
        BookShelfChild.SetActive(true);        
        
    }
}