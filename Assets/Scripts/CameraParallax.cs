using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParallax : MonoBehaviour
{
    public float positionAmount = 0.1f; // ī�޶� �̵� ���� (���� �������� �ʰ� ����)
    public float smoothSpeed = 5f; // �ε巯�� ��ȯ �ӵ�


    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // ���콺 ��ġ�� ȭ�� ������ ��ȯ (-1 ~ 1 ����)
        Vector2 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float xOffset = (mousePos.x / screenWidth - 0.5f) * 2f;
        float yOffset = (mousePos.y / screenHeight - 0.5f) * 2f;


        // ��ǥ ��ġ ��� (�ʱ� ��ġ���� �ణ�� �̵�)
        Vector3 targetPosition = initialPosition + new Vector3(xOffset * positionAmount, yOffset * positionAmount, 0);

        // �ε巴�� �����Ͽ� ����        
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);


    }
}
