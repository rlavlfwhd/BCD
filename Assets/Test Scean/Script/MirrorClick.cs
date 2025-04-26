using UnityEngine;

public class MirrorClick : MonoBehaviour
{
    public GameObject backButton;

    public GameObject[] objects;

    private void Start()
    {
        CheckBackButton();
    }

    public void CheckBackButton()
    {
        bool anyObjectActive = false;

        foreach (GameObject obj in objects)
        {
            if (obj.activeSelf)
            {
                anyObjectActive = true;
                break;
            }
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
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        CheckBackButton();
    }

    void OnMouseDown()
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }

        CheckBackButton();
    }
}
