using UnityEngine;
using System.Collections;

public class MoleController : MonoBehaviour, IClickablePuzzle
{
    [Header("💬 대사 목록")]
    [TextArea]
    public string[] dialogueLines;

    [Header("💬 말푸선 기준 위치")]
    public Transform speechBubbleAnchor;

    [Header("🎯 컨퍼질 비율 & 속도")]
    public float selectedScale = 1.3f;
    public float scaleSpeed = 5f;

    [Header("🧩 정답인지 여분")]
    public bool isAnswer = false;

    [Header("🕳️ 가이드인지 여분")]
    public bool isGuide = false;

    private static readonly string puzzleID = "MolePuzzle";

    private int currentDialogueIndex = 0;
    private bool isDialogueFinished = false;
    private Vector3 originalScale;
    private Coroutine scaleRoutine;
    private bool isPuzzleCompleted = false;

    private void OnEnable()
    {
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            MolePuzzleFailManager.Instance.HandleSuccess();
        }
        else
        {
            StartCoroutine(InitializePuzzleState());

        }
    }
    private IEnumerator InitializePuzzleState()
    {
        yield return new WaitUntil(() => PuzzleManager.Instance != null);
        yield return null;

        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            isPuzzleCompleted = true;
        }
    }

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnClickPuzzle()
    {
        if (isPuzzleCompleted) return;

        MolePuzzleManager.Instance.SelectMole(this);
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
        if (!MolePuzzleManager.Instance.canChooseAnswer) return;

        if (isAnswer)
        {
            Debug.Log("정답! 🎯");
            PuzzleManager.Instance.CompletePuzzle(puzzleID);
            isPuzzleCompleted = true;
            MolePuzzleFailManager.Instance.HandleSuccess();
        }
        else
        {
            Debug.Log("오단! 패익!");
            MolePuzzleFailManager.Instance.HandleFail();
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

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
            yield return null;
        }
        transform.localScale = targetScale;
    }
}