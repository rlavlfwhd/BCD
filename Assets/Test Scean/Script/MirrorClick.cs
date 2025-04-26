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
