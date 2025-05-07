using UnityEngine;

public class BookShelfClickHandler : MonoBehaviour, IClickablePuzzle
{
    public GameObject bookShelfChild;
    public FadeUI fadeUI; // ✅ 추가: 페이드 UI 연결용 변수

    public void OnClickPuzzle()
    {
        if (bookShelfChild != null)
        {
            bookShelfChild.SetActive(true);
        }

        // ✅ FadeUI가 연결돼 있다면 페이드 효과 실행
        if (fadeUI != null)
        {
            fadeUI.Play();
        }
    }
}
