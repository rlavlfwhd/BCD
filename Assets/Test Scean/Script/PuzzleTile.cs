using UnityEngine;

public class PuzzleTile : MonoBehaviour
{
    public bool isOn = false;

    private Material _materialInstance;
    private MeshRenderer _renderer;

    public Color onColor = Color.green;
    public Color offColor = Color.gray;

    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();

        // 머티리얼 인스턴스화 (각 타일 개별 색 변경 가능)
        _materialInstance = new Material(_renderer.material);
        _renderer.material = _materialInstance;

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (_materialInstance != null)
        {
            _materialInstance.color = isOn ? onColor : offColor;
        }
    }
}

