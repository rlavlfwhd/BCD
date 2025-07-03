using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimator : MonoBehaviour
{
    public float floatSpeed = 1f;         // 움직이는 속도
    public float floatHeight = 0.2f;      // 움직이는 높이
    public float autoHideTime = 5f;       // 사라지는 시간(초)

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        startPos = transform.position;
        timer = 0f;
    }

    void Update()
    {
        // 1. 위아래로 움직임
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // 2. 시간 경과에 따라 오브젝트 삭제
        timer += Time.deltaTime;
        if (timer >= autoHideTime)
        {
            Destroy(gameObject); // ?? 오브젝트 자체 삭제
        }
    }

    // 외부에서 화살표를 다시 활성화할 때 초기화
    public void ResetArrow()
    {
        timer = 0f;
        transform.position = startPos;
        gameObject.SetActive(true);
    }
}
