using UnityEngine;

/// <summary>
/// 펼친 편지에 붙여두고, 클릭하면 자신을 비활성화합니다.
/// </summary>
public class LetterViewSimple : MonoBehaviour
{
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        Debug.Log("[LetterViewSimple] 편지 닫기");
    }
}
