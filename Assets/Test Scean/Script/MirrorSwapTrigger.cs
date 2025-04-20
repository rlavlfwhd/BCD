using UnityEngine;

public class MirrorSwapTrigger : MonoBehaviour
{
    public GameObject mirrorOriginal;  // 기존 Mirror 오브젝트
    public GameObject mirrorAlternate; // 바뀔 Mirror2 오브젝트

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chicken")) // 닭 오브젝트에 "Chicken" 태그 설정!
        {
            if (mirrorOriginal != null) mirrorOriginal.SetActive(false);
            if (mirrorAlternate != null) mirrorAlternate.SetActive(true);

            Debug.Log("닭이 거울에 접근 → Mirror 교체됨");
        }
    }
}
