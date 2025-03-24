using UnityEngine;

public class FixedWorldOnOffUI : MonoBehaviour
{
    private Transform cam; // 카메라 참조
    private Vector3 initialOffset; // 초기 UI 위치 오프셋
    public float distanceFromCamera = 5f; // 카메라에서 얼마나 떨어질지

    void Start()
    {
        cam = Camera.main.transform; // 메인 카메라 찾기
        SetUIPosition(); // 시작할 때 한 번 위치 세팅
    }

    void OnEnable()
    {
        SetUIPosition(); // UI가 활성화될 때 위치 조정
    }

    void SetUIPosition()
    {
        if (cam == null) return;

        // 카메라 앞쪽(distanceFromCamera만큼 떨어진 곳)에 배치
        transform.position = cam.position + cam.forward * distanceFromCamera;

        // UI가 카메라를 정면으로 바라보게 설정
        transform.rotation = cam.rotation;
    }

    void LateUpdate()
    {
        // UI가 켜진 후에도 카메라 위치를 따라가도록 유지
        transform.position = cam.position + cam.forward * distanceFromCamera;
        transform.rotation = cam.rotation;
    }
}
