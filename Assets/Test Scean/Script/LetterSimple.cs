using UnityEngine;

/// <summary>
/// ������ �ٿ��ΰ�, Ŭ���ϸ� ��ģ ���� ������Ʈ�� Ȱ��ȭ�մϴ�.
/// </summary>
public class LetterSimple : MonoBehaviour
{
    [Header("Inspector�� ������ ��ģ ���� GameObject")]
    public GameObject openedLetter;

    private void OnMouseDown()
    {
        if (openedLetter != null)
        {
            openedLetter.SetActive(true);
            Debug.Log("[LetterSimple] ���� ��ħ Ȱ��ȭ");
        }
    }
}
