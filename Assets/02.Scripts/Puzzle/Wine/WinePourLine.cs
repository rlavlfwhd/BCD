using UnityEngine;

public class WinePourLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform startPoint;   // 와인병 위치
    public Transform endPoint;     // 쉐이커 위치
    public float pourSpeed = 2f;   // 내려오는 속도

    private float currentLength = 0f;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, startPoint.position);
    }

    void Update()
    {
        if (currentLength < 1f)
        {
            currentLength += Time.deltaTime * pourSpeed;
            Vector3 targetPos = Vector3.Lerp(startPoint.position, endPoint.position, currentLength);
            lineRenderer.SetPosition(1, targetPos);
        }
    }
}

