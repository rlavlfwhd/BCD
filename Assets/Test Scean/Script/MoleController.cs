using UnityEngine;
using UnityEngine.SceneManagement;

public class MoleController : MonoBehaviour
{
    [Header("💬 대사 목록")]
    [TextArea]
    public string[] dialogueLines;

    [Header("💬 말풍선 기준 위치")]
    public Transform speechBubbleAnchor;

    [Header("🎯 커지는 비율 & 속도")]
    public float selectedScale = 1.3f;
    public float scaleSpeed = 5f;

    [Header("🧩 정답인지 여부")]
    public bool isAnswer = false;

    [Header("👓 가이드인지 여부")]
    public bool isGuide = false;

    private int currentDialogueIndex = 0;
    private bool isDialogueFinished = false;
    private Vector3 originalScale;
    private Coroutine scaleRoutine;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        MolePuzzleManager.Instance.SelectMole(this); // 크기 선택
        ShowDialogue();

        if (isGuide) return;

        if (MolePuzzleManager.Instance.canChooseAnswer)
        {
            CheckIfCorrect();
        }
    }

    private void ShowDialogue()
    {
        if (dialogueLines.Length == 0 || speechBubbleAnchor == null) return;

        string message = dialogueLines[currentDialogueIndex];
        SpeechBubbleManager.Instance.ShowBubble(this, speechBubbleAnchor, message);

        if (isGuide && currentDialogueIndex == 4)
        {
            MolePuzzleManager.Instance.AllowAnswerSelection();
        }

        currentDialogueIndex = (currentDialogueIndex + 1) % dialogueLines.Length;
        isDialogueFinished = true;
    }

    public void OnDialogueComplete()
    {
        isDialogueFinished = false;
    }

    private void CheckIfCorrect()
    {
        if (!MolePuzzleManager.Instance.canChooseAnswer)
        {
            Debug.Log("⚠️ 아직 정답 선택 불가 (가이드의 대사 필요)");
            return;
        }

        if (isAnswer)
        {
            Debug.Log("정답! 🎯");

            if (MolePuzzleFailManager.Instance != null)
            {
                MolePuzzleFailManager.Instance.HandleSuccess(); // ✅ 성공 → 여기로 위임
            }
            else
            {
                Debug.LogWarning("⚠️ MolePuzzleFailManager.Instance 없음!");
            }
        }
        else
        {
            Debug.Log("❌ 오답! 실패 처리 실행");

            if (MolePuzzleFailManager.Instance != null)
            {
                MolePuzzleFailManager.Instance.HandleFail(); // ✅ 실패 → 여기로 위임
            }
            else
            {
                Debug.LogWarning("⚠️ MolePuzzleFailManager.Instance 없음!");
            }
        }
    }

    public void Select()
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(ScaleTo(originalScale * selectedScale));
    }

    public void ResetScale()
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(ScaleTo(originalScale));
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}

