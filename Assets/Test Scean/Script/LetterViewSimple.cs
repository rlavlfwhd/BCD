using UnityEngine;

/// <summary>
/// ��ģ ������ �ٿ��ΰ�, Ŭ���ϸ� �ڽ��� ��Ȱ��ȭ�մϴ�.
/// </summary>
public class LetterViewSimple : MonoBehaviour
{
    private void OnMouseDown()
    {
        gameObject.SetActive(false);
        Debug.Log("[LetterViewSimple] ���� �ݱ�");
    }
}
