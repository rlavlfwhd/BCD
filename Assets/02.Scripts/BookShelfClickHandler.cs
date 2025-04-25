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
                backButton.SetActive(true); // ������Ʈ�� ���� �ְ�, �ſ� �� ���� ���� ����
            }
            else
            {
                backButton.SetActive(false); // ������Ʈ�� ���� �־ �ſ� ������ �� ����
            }
        }
        else
        {
            backButton.SetActive(false); // ������Ʈ ���� ������ ������ �� ����
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
