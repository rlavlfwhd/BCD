using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimator : MonoBehaviour
{
    public float floatSpeed = 1f;         // �����̴� �ӵ�
    public float floatHeight = 0.2f;      // �����̴� ����
    public float autoHideTime = 5f;       // ������� �ð�(��)

    private Vector3 startPos;
    private float timer;

    void Start()
    {
        startPos = transform.position;
        timer = 0f;
    }

    void Update()
    {
        // 1. ���Ʒ��� ������
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // 2. �ð� ����� ���� ������Ʈ ����
        timer += Time.deltaTime;
        if (timer >= autoHideTime)
        {
            Destroy(gameObject); // ?? ������Ʈ ��ü ����
        }
    }

    // �ܺο��� ȭ��ǥ�� �ٽ� Ȱ��ȭ�� �� �ʱ�ȭ
    public void ResetArrow()
    {
        timer = 0f;
        transform.position = startPos;
        gameObject.SetActive(true);
    }
}
