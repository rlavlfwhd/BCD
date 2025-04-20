using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 1.0f; // ���ٴϴ� �ӵ� ������ ���� ����
    public float floatAmount = 20f; // ���ٴϴ� ���� ������ ���� ����
    private Vector2 startPos; // �ʱ� ��ġ ������ ���� ����

    void Start()
    {
        // UI ����� �ʱ� ��ġ�� ����
        startPos = GetComponent<RectTransform>().anchoredPosition;
    }

    void Update()
    {
        // UI ��Ҹ� ���ٴϰ� ����� ���� sin �Լ��� ����Ͽ� Y�࿡ ���� ��ġ�� ���
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;

        // ���� Y�� ��ġ�� UI ����� ��ġ�� ������Ʈ
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, newY);
    }
}
