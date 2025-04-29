using UnityEngine;

public class MolePuzzleManager : MonoBehaviour
{
    public static MolePuzzleManager Instance;

    [Header("퍼즐 설정")]
    public int correctMoleIndex; // 정답 두더지 번호 (0~2)

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OnGuideClicked()
    {
        Debug.Log("🧸 안내자 클릭: 거짓말쟁이를 찾았니?");

        // (나중에 UI 띄우거나 선택 입력 받게 할 수 있음)
        // 여기서 정답 선택 창 열기
    }

    public void CheckAnswer(int selectedIndex)
    {
        if (selectedIndex == correctMoleIndex)
        {
            Debug.Log("🎉 정답! 퍼즐 성공!");
            // 퍼즐 클리어 처리
        }
        else
        {
            Debug.Log("❌ 틀렸어! 다시 찾아봐!");
            // 퍼즐 실패 처리
        }
    }
}
