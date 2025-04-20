using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleChecker : MonoBehaviour
{
    // ���� ���� ����
    private readonly string[] correctPattern = { "RedCircle", "YellowTriangle", "BlueHalfCircle" };

    // �÷��̾ ��ġ�� ������Ʈ�� (�巡�� �� ���Կ� �� ����)
    public List<string> playerPattern = new List<string>();

    // ���� �Ϸ� ����
    public bool isPuzzleComplete = false;

    // ȣ��: ������ ��� ä������ ��
    public void CheckPuzzle()
    {
        if (playerPattern.Count != correctPattern.Length)
        {
            Debug.LogWarning("���� ���� ���� ����");
            return;
        }

        for (int i = 0; i < correctPattern.Length; i++)
        {
            if (playerPattern[i] != correctPattern[i])
            {
                Debug.Log("Ʋ�Ƚ��ϴ�!");
                return;
            }
        }

        Debug.Log("���� ����! �����Դϴ�!");
        isPuzzleComplete = true;
        // �̰��� ���� �Ϸ� �̺�Ʈ �߰� (�� ���� ��)
    }
}
