// 🍷 WinePuzzleManager.cs 완성본 (주석 하나하나 매우 자세하게 달림)
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퍼즐의 정답 와인 순서를 관리하고, 플레이어 입력을 검사하는 매니저 스크립트
/// </summary>
public class WinePuzzleManager : MonoBehaviour
{
    [Header("✅ 정답 와인 순서 (Inspector에서 순서대로 넣기)")]
    public List<string> correctWineOrder; // 정답 순서를 색상 이름(string)으로 보관 (예: "Gold", "Red", "Green")

    // 👉 플레이어가 선택한 와인 순서를 저장할 리스트
    private List<string> selectedWineOrder = new List<string>();

    [Header("✅ 쉐이커 컨트롤러 (흔들림 연출용)")]
    public ShakeController shakeController; // 쉐이커 움직임 담당 스크립트 연결

    /// <summary>
    /// 플레이어가 와인 병 하나를 클릭할 때마다 호출 (색상 이름을 전달받음)
    /// </summary>
    /// <param name="wineColor">선택된 와인의 색상 이름</param>
    public void SelectWine(string wineColor)
    {
        // 👉 선택한 색상 이름을 리스트에 추가
        selectedWineOrder.Add(wineColor);

        // 👉 현재까지 선택된 순서 디버그 출력
        Debug.Log($"현재 선택 순서: {string.Join(", ", selectedWineOrder)}");

        // 👉 선택된 갯수가 정답 갯수와 같으면 정답 검사 실행
        if (selectedWineOrder.Count == correctWineOrder.Count)
        {
            CheckSequence();
        }
    }

    /// <summary>
    /// 선택된 와인 순서가 정답과 일치하는지 검사하는 메서드
    /// </summary>
    private void CheckSequence()
    {
        bool isCorrect = true; // 초기엔 맞다고 가정

        // 👉 선택한 순서와 정답 순서를 하나하나 비교
        for (int i = 0; i < correctWineOrder.Count; i++)
        {
            if (selectedWineOrder[i] != correctWineOrder[i])
            {
                isCorrect = false; // 하나라도 틀리면 false
                break; // 반복 중단
            }
        }

        if (isCorrect)
        {
            Debug.Log("🎉 정답입니다! 쉐이커를 흔듭니다.");

            // 👉 쉐이커 흔들기 실행 (연결된 ShakeController가 있으면)
            if (shakeController != null)
            {
                shakeController.StartShaking();
            }
            else
            {
                Debug.LogWarning("⚠ ShakeController가 Inspector에 연결되지 않았습니다.");
            }
        }
        else
        {
            Debug.Log("❌ 틀렸습니다. 다시 시도하세요.");
        }

        // 👉 선택 리스트 초기화 (다음 시도 준비)
        selectedWineOrder.Clear();
    }
}
