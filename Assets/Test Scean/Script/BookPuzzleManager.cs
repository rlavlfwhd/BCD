using UnityEngine;
using System.Collections.Generic;

public class BookPuzzleManager : MonoBehaviour
{
    public List<string> correctOrder = new List<string> { "Red", "Yellow", "Blue", "Green" };
    private List<string> currentOrder = new List<string>();

    public void RegisterBook(string color)
    {
        if (currentOrder.Count >= correctOrder.Count)
            return;

        currentOrder.Add(color);
        CheckOrder();
    }

    void CheckOrder()
    {
        for (int i = 0; i < currentOrder.Count; i++)
        {
            if (currentOrder[i] != correctOrder[i])
            {
                Debug.Log("틀렸습니다. 다시 시도하세요.");
                currentOrder.Clear();
                return;
            }
        }

        if (currentOrder.Count == correctOrder.Count)
        {
            Debug.Log("퍼즐 클리어!");
            // 클리어 처리 로직 추가 (예: 문 열기, 레이어 전환 등)
        }
    }
}
