using UnityEngine;
using TMPro;

public class PrincessDialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // 공주 대사 텍스트창
    public string[] dialogues;           // 대사 리스트
    private int currentIndex = 0;

    // 외부에서 호출하여 대사 진행
    public void ShowNextDialogue()
    {
        if (currentIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentIndex];
            currentIndex++;
        }
        else
        {
            dialogueText.text = "대사가 끝났습니다.";
            // 다음 이벤트나 씬 전환 등을 처리할 수 있어요.
        }
    }

    // 예: 대사 초기화
    public void SetDialogue(string[] newDialogue)
    {
        dialogues = newDialogue;
        currentIndex = 0;
        ShowNextDialogue(); // 첫 대사 자동 표시
    }
}
