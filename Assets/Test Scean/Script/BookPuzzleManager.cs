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
                Debug.Log("Ʋ�Ƚ��ϴ�. �ٽ� �õ��ϼ���.");
                currentOrder.Clear();
                return;
            }
        }

        if (currentOrder.Count == correctOrder.Count)
        {
            Debug.Log("���� Ŭ����!");
            // Ŭ���� ó�� ���� �߰� (��: �� ����, ���̾� ��ȯ ��)
        }
    }
}
