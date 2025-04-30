using UnityEngine;

public class MoleController : MonoBehaviour
{
    public enum MoleType { TruthTeller, Liar, HalfLiar, Guide }
    public MoleType moleType;

    [Header("💬 대사 목록")]
    [TextArea]
    public string[] dialogueLines;

    [Header("🧭 말풍선 위치 오프셋")]
    public Vector3 bubbleOffset = new Vector3(0f, 1.5f, 0f);

    private int currentDialogueIndex = 0;

    private void OnMouseDown()
    {
        Debug.Log($"🖱️ [클릭됨] {gameObject.name} 두더지를 클릭했습니다.");
        OnClicked();
    }

    public void OnClicked()
    {
        Debug.Log($"✅ [OnClicked 호출] MoleType: {moleType}");

        ShowDialogue();

        if (moleType == MoleType.Guide && MolePuzzleManager.Instance != null)
        {
            MolePuzzleManager.Instance.OnGuideClicked();
        }
    }

    private void ShowDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            Debug.LogWarning($"⚠️ {gameObject.name}: 대사 배열이 비어 있습니다.");
            return;
        }

        string line = dialogueLines[currentDialogueIndex];
        Debug.Log($"💬 [대사 출력] {gameObject.name}: \"{line}\"");

        // ✅ SpeechBubbleManager 진단 로그
        if (SpeechBubbleManager.Instance == null)
        {
            Debug.LogError($"❌ SpeechBubbleManager.Instance is NULL. 말풍선 생성 불가!");
            return;
        }

        Debug.Log($"🧪 [ShowBubble 호출 전] SpeechBubbleManager 존재 확인 완료. 말풍선 출력 시도 중...");
        SpeechBubbleManager.Instance.ShowBubble(transform, line, bubbleOffset);

        currentDialogueIndex = (currentDialogueIndex + 1) % dialogueLines.Length;
    }
}




