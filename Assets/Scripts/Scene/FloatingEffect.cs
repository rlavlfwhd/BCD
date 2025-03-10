using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 1.0f; // 떠다니는 속도 조절을 위한 변수
    public float floatAmount = 20f; // 떠다니는 간격 조절을 위한 변수
    private Vector2 startPos; // 초기 위치 저장을 위한 변수

    void Start()
    {
        // UI 요소의 초기 위치를 저장
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        // UI 요소를 떠다니게 만들기 위해 sin 함수를 사용하여 Y축에 대한 위치를 계산
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        // 계산된 Y축 위치로 UI 요소의 위치를 업데이트
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}
