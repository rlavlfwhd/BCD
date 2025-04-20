using UnityEngine;

public class FixedWorldUI : MonoBehaviour
{
    private Transform cam; // 카메라 참조
    private Vector3 initialOffset; // 초기 UI 위치 오프셋

    void Start()
    {
        cam = Camera.main.transform; // 메인 카메라 찾기
        initialOffset = transform.position - cam.position; // 처음 거리 저장
    }

    void LateUpdate()
    {
        // 카메라 위치를 따라가지만 회전은 유지
        transform.position = cam.position + initialOffset;
        transform.rotation = Quaternion.identity; // 회전 고정
    }
}