using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    public float positionAmount = 0.1f; // 카메라 이동 정도 (거의 움직이지 않게 설정)
    public float smoothSpeed = 5f; // 부드러운 전환 속도


    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // 마우스 위치를 화면 비율로 변환 (-1 ~ 1 범위)
        Vector2 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float xOffset = (mousePos.x / screenWidth - 0.5f) * 2f;
        float yOffset = (mousePos.y / screenHeight - 0.5f) * 2f;


        // 목표 위치 계산 (초기 위치에서 약간만 이동)
        Vector3 targetPosition = initialPosition + new Vector3(xOffset * positionAmount, yOffset * positionAmount, 0);

        // 부드럽게 보간하여 적용        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);


    }
}
