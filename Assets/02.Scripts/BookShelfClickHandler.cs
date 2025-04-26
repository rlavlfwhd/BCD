using UnityEngine;

public class BookShelfClickHandler : MonoBehaviour
{
    public GameObject backButton;

    public GameObject BookShelfChild;

    private void Start()
    {
        CheckBackButton();
    }

    public void CheckBackButton()
    {
        bool anyObjectActive = false;

            if (BookShelfChild.activeSelf)
            {
                anyObjectActive = true;
            }
       
        if (anyObjectActive)
        {
            if (!SceneDataManager.Instance.Data.isMirrorFlipped)
            {
                backButton.SetActive(true); // 오브젝트가 켜져 있고, 거울 안 깼을 때만 보임
            }
            else
            {
                backButton.SetActive(false); // 오브젝트가 켜져 있어도 거울 깼으면 안 보임
            }
        }
        else
        {
            backButton.SetActive(false); // 오브젝트 꺼져 있으면 무조건 안 보임
        }
    }

    public void BackBtn()
    {
        BookShelfChild.SetActive(false);
        CheckBackButton();
    }

    void OnMouseDown()
    {
        BookShelfChild.SetActive(true);        
        CheckBackButton();
    }
}
