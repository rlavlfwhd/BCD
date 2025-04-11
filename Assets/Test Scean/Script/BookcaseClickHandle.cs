using UnityEngine;
using UnityEngine.UI;

public class BookcaseClickHandler : MonoBehaviour
{
    public Image targetImage;    // ������ �̹��� (0Layer ���� Image)
    public Sprite newSprite;     // ��ü�� ���ο� ��� ��������Ʈ
    public GameObject[] objectsToHide; // ���� ������Ʈ���� �迭�� �ޱ�

    void OnMouseDown()
    {
        if (targetImage != null && newSprite != null)
        {
            targetImage.sprite = newSprite;

            // ���� ������Ʈ ��Ȱ��ȭ
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(false);
            }

            Debug.Log("å�� Ŭ�� �� ��� ��ü + ������Ʈ ���� �Ϸ�!");
        }
    }
}