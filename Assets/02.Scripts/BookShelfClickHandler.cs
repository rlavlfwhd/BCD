using UnityEngine;

public class BookShelfClickHandler : MonoBehaviour, IClickablePuzzle
{
    public GameObject bookShelfChild;

    public void OnClickPuzzle()
    {
        if(bookShelfChild != null)
        {
            bookShelfChild.SetActive(true);
        }
    }
}