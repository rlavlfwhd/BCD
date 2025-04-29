using UnityEngine;

public class MoleController : MonoBehaviour
{
    public enum MoleType { TruthTeller, Liar, HalfLiar, Guide }
    public MoleType moleType;

    [TextArea]
    public string[] dialogueLines;
    private int currentDialogueIndex = 0;

    private void OnMouseDown()
    {
        Debug.Log($"🖱️ [클릭됨] {gameObject.name} 두더지를 클릭했습니다.");
        OnClicked();
    }

    public void OnClicked()
    {
        Debug.Log($"✅ [OnClicked 호출] MoleType: {moleType}");

        ShowDialogue(); // 🌟 무조건 대사는 보여준다.

        if (moleType == MoleType.Guide)
        {
            MolePuzzleManager.Instance.OnGuideClicked(); // 🌟 추가로 가이드 역할도 같이 수행
        }
    }

    private void ShowDialogue()
    {
        if (dialogueLines.Length > 0)
        {
            string line = dialogueLines[currentDialogueIndex];
            Debug.Log($"💬 [대사 출력] {gameObject.name}: \"{line}\"");

            currentDialogueIndex++;

            if (currentDialogueIndex >= dialogueLines.Length)
            {
                currentDialogueIndex = 0;
            }
        }
        else
        {
            Debug.LogWarning($"⚠️ {gameObject.name}: 대사 데이터가 없습니다.");
        }
    }
}

