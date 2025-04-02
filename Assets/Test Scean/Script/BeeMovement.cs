using UnityEngine;

public class PerfectBeeOrbit : MonoBehaviour
{
    public Transform target; // 공주의 위치
    public float radius = 2f; // 공주를 중심으로 도는 반지름
    public float speed = 2f; // 회전 속도 (초당 회전 각도)
    private float angle = 0f; // 현재 각도

    void Update()
    {
        if (target == null)
        {
            Debug.LogError("target(공주)이 설정되지 않았습니다. 씬에서 target을 설정해주세요.");
            return;
        }

        // 회전 각도 증가
        angle += speed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f; // 각도 값이 너무 커지지 않도록 보정

        // 공주를 중심으로 원형 좌표 계산
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        // 벌 위치 설정
        transform.position = target.position + new Vector3(x, y, 0);
    }
}
