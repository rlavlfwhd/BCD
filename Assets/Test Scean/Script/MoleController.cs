using UnityEngine;

public class MoleController : MonoBehaviour
{
    public enum MoleType { TruthTeller, Liar, HalfLiar, Guide }
    public MoleType moleType;

    [TextArea]
    public string[] dialogueLines; // 두더지 대사들 (2개)
    private int currentDialogueIndex = 0;

    private bool hasTalked = false;

    public void OnClicked()
    {
        if (moleType == MoleType.Guide)
        {
            MolePuzzleManager.Instance.OnGuideClicked();
        }
        else
        {
            if (!hasTalked)
            {
                ShowDialogue();
            }
        }
    }

    private void ShowDialogue()
    {
        if (currentDialogueIndex < dialogueLines.Length)
        {
            string line = dialogueLines[currentDialogueIndex];
            Debug.Log($"🧸 {gameObject.name} 대사: {line}");
            currentDialogueIndex++;

            if (currentDialogueIndex >= dialogueLines.Length)
            {
                hasTalked = true; // 대사 다 했으면 더 이상 안 함
            }
        }
    }
}
