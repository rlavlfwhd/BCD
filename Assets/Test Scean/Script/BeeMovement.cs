using UnityEngine;

public class PerfectBeeOrbit : MonoBehaviour
{
    public Transform target; // ������ ��ġ
    public float radius = 2f; // ���ָ� �߽����� ���� ������
    public float speed = 2f; // ȸ�� �ӵ� (�ʴ� ȸ�� ����)
    private float angle = 0f; // ���� ����

    void Update()
    {
        if (target == null)
        {
            Debug.LogError("target(����)�� �������� �ʾҽ��ϴ�. ������ target�� �������ּ���.");
            return;
        }

        // ȸ�� ���� ����
        angle += speed * Time.deltaTime;
        if (angle >= 360f) angle -= 360f; // ���� ���� �ʹ� Ŀ���� �ʵ��� ����

        // ���ָ� �߽����� ���� ��ǥ ���
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

        // �� ��ġ ����
        transform.position = target.position + new Vector3(x, y, 0);
    }
}
