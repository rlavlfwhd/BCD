using UnityEngine;
using UnityEngine.UI;

public class BookcaseClickHandler : MonoBehaviour
{
    public Image targetImage;    // 변경할 이미지 (0Layer 안의 Image)
    public Sprite newSprite;     // 교체할 새로운 배경 스프라이트
    public GameObject[] objectsToHide; // 숨길 오브젝트들을 배열로 받기

    void OnMouseDown()
    {
        if (targetImage != null && newSprite != null)
        {
            targetImage.sprite = newSprite;

            // 기존 오브젝트 비활성화
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(false);
            }

            Debug.Log("책장 클릭 시 배경 교체 + 오브젝트 숨김 완료!");
        }
    }
}