using UnityEngine;

public class LayerBackHandler : MonoBehaviour
{
    public GameObject layer0; // 다시 켜질 0Layer
    public GameObject layer1; // 다시 꺼질 1Layer
    public GameObject[] objectsToShow; // 다시 보여줄 오브젝트들

    void OnMouseDown()
    {
        if (layer1 != null) layer1.SetActive(false);
        if (layer0 != null) layer0.SetActive(true);

        foreach (GameObject obj in objectsToShow)
        {
            obj.SetActive(true);
        }

        Debug.Log("이동 오브젝트 클릭 시 0Layer 복귀 + 숨긴 오브젝트 다시 표시 완료!");
    }
}
