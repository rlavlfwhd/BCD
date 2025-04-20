using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleChecker : MonoBehaviour
{
    // 정답 순서 정의
    private readonly string[] correctPattern = { "RedCircle", "YellowTriangle", "BlueHalfCircle" };

    // 플레이어가 배치한 오브젝트들 (드래그 후 슬롯에 들어간 순서)
    public List<string> playerPattern = new List<string>();

    // 퍼즐 완료 여부
    public bool isPuzzleComplete = false;

    // 호출: 슬롯이 모두 채워졌을 때
    public void CheckPuzzle()
    {
        if (playerPattern.Count != correctPattern.Length)
        {
            Debug.LogWarning("슬롯 수가 맞지 않음");
            return;
        }

        for (int i = 0; i < correctPattern.Length; i++)
        {
            if (playerPattern[i] != correctPattern[i])
            {
                Debug.Log("틀렸습니다!");
                return;
            }
        }

        Debug.Log("퍼즐 성공! 정답입니다!");
        isPuzzleComplete = true;
        // 이곳에 퍼즐 완료 이벤트 추가 (문 열림 등)
    }
}
