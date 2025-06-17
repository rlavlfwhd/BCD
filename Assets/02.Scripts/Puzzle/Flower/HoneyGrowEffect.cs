using UnityEngine;

public class HoneyGrowEffect : MonoBehaviour
{
    [Header("�� ���� ����")]
    [Tooltip("������ �� �ܹ���� ũ���Դϴ�.")]
    public Vector3 initialScale = new Vector3(0.5f, 0.5f, 0.5f);

    [Tooltip("���� �Ϸ� �� �ܹ���� ���� ũ���Դϴ�.")]
    public Vector3 targetScale = Vector3.one;

    [Tooltip("ũ�Ⱑ Ŀ���� �� �ɸ��� �ð�(��)�Դϴ�.")]
    public float growTime = 0.5f;

    private float timer = 0f;

    void Start()
    {
        transform.localScale = initialScale; // ������ �� ������ ũ��� ����
    }

    void Update()
    {
        if (timer < growTime)
        {
            timer += Time.deltaTime;
            float t = timer / growTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
        }
    }
}

