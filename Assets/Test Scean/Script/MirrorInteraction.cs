using UnityEngine;

public class MirrorInteraction : MonoBehaviour
{
    // Layer1 오브젝트 그룹 (기본 화면)
    public GameObject layer1Objects;

    // Layer2 오브젝트 그룹 (거울 클릭 후 전환되는 화면)
    public GameObject layer2Objects;

    // 현재 상태를 저장하는 변수
    private bool isInLayer2 = false;

    void OnMouseDown()
    {
        // 현재 상태가 Layer2인지 확인
        if (!isInLayer2)
        {
            // Layer1 비활성화
            if (layer1Objects != null)
                layer1Objects.SetActive(false);

            // Layer2 활성화
            if (layer2Objects != null)
                layer2Objects.SetActive(true);

            Debug.Log("거울 클릭: Layer2로 전환됨");
        }
        else
        {
            // Layer1 다시 활성화
            if (layer1Objects != null)
                layer1Objects.SetActive(true);

            // Layer2 비활성화
            if (layer2Objects != null)
                layer2Objects.SetActive(false);

            Debug.Log("거울 클릭: Layer1로 돌아감");
        }

        // 상태 반전
        isInLayer2 = !isInLayer2;
    }
}
