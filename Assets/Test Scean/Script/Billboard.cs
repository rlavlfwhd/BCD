using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        // 메인 카메라 참조
        cam = Camera.main;

        // 예외 방지: 카메라가 없는 경우
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
            Debug.LogWarning("⚠️ Billboard: Main Camera가 지정되지 않아 첫 번째 Camera를 찾았습니다.");
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // 카메라를 바라보게 회전
        transform.LookAt(cam.transform);

        // UI가 뒷면을 보는 문제를 막기 위한 보정 회전 (180도 뒤집기)
        transform.Rotate(0f, 180f, 0f);
    }
}
