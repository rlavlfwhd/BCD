using UnityEngine;
using TMPro;

public class PrincessDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // ���� ��� �ؽ�Ʈâ
    public string[] dialogues;           // ��� ����Ʈ
    private int currentIndex = 0;

    // �ܺο��� ȣ���Ͽ� ��� ����
    public void ShowNextDialogue()
    {
        if (currentIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentIndex];
            currentIndex++;
        }
        else
        {
            dialogueText.text = "��簡 �������ϴ�.";
            // ���� �̺�Ʈ�� �� ��ȯ ���� ó���� �� �־��.
        }
    }

    // ��: ��� �ʱ�ȭ
    public void SetDialogue(string[] newDialogue)
    {
        dialogues = newDialogue;
        currentIndex = 0;
        ShowNextDialogue(); // ù ��� �ڵ� ǥ��
    }
}
