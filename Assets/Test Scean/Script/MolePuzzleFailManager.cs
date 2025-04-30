using UnityEngine;

public class MolePuzzleFailManager : MonoBehaviour
{
    [Header("🕶 페이드 연출 담당")]
    public FadeController fadeController; // FadeController가 붙은 오브젝트 (F_Image 등)

    /// <summary>
    /// 오답 시 호출: 짧은 대사 출력 + 페이드 아웃 + 씬 재시작
    /// </summary>
    public void HandleFail()
    {
        if (fadeController != null)
        {
            Debug.Log("❌ 오답 → 실패 연출 시작");
            fadeController.ShowFailureDialogueThenRestart("어림도 없지 암!!");
        }
        else
        {
            Debug.LogWarning("⚠️ FadeController가 연결되어 있지 않음!");
        }
    }
}
