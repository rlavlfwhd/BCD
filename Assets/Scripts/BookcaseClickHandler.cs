using UnityEngine;

public class BookcaseClickHandler : MonoBehaviour
{
    public GameObject layer0; // 0Layer 오브젝트
    public GameObject layer1; // 1Layer 오브젝트
    public GameObject[] objectsToHide; // 클릭 시 숨길 다른 오브젝트들

    void OnMouseDown()
    {
        if (layer0 != null) layer0.SetActive(false);
        if (layer1 != null) layer1.SetActive(true);

        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }

        Debug.Log("책장 클릭 시 1Layer 배경 활성화 + 기존 오브젝트 숨김 완료!");
    }
}
