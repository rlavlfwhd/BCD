using UnityEngine;

/// <summary>
/// 봉투에 붙여두고, 클릭하면 펼친 편지 오브젝트를 활성화합니다.
/// </summary>
public class LetterSimple : MonoBehaviour
{
    [Header("Inspector에 연결할 펼친 편지 GameObject")]
    public GameObject openedLetter;

    private void OnMouseDown()
    {
        if (openedLetter != null)
        {
            openedLetter.SetActive(true);
            Debug.Log("[LetterSimple] 편지 펼침 활성화");
        }
    }
}
