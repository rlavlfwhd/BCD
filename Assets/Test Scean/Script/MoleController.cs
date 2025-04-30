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

    [Header("🎬 정답 시 이동할 씬 이름")]
    public string nextSceneName;

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
        // ✅ 안정적 정답 판정: 정답인 경우, 선택 가능 상태면 즉시 처리
        if (!isGuide && isAnswer && MolePuzzleManager.Instance.canChooseAnswer)
        {
            CheckIfCorrect(); // 대사 출력 없이 즉시 정답 처리
            return;
        }

        if (!isDialogueFinished)
        {
            MolePuzzleManager.Instance.SelectMole(this);
            ShowDialogue();
        }
        else
        {
            ShowDialogue(); // 대사 계속 순환

            if (!isGuide)
                CheckIfCorrect(); // 일반 두더지 정답 판정
        }
    }

    private void ShowDialogue()
    {
        if (dialogueLines.Length == 0 || speechBubbleAnchor == null) return;

        string message = dialogueLines[currentDialogueIndex];
        SpeechBubbleManager.Instance.ShowBubble(this, speechBubbleAnchor, message);

        // ✅ 가이드의 3번째 대사 출력 시 정답 선택 가능 상태로 전환
        if (isGuide && currentDialogueIndex == 2)
        {
            MolePuzzleManager.Instance.AllowAnswerSelection();
        }

        currentDialogueIndex = (currentDialogueIndex + 1) % dialogueLines.Length;
        isDialogueFinished = true;
    }

    public void OnDialogueComplete()
    {
        isDialogueFinished = false; // 대사 끝나면 다시 클릭 가능
    }

    private void CheckIfCorrect()
    {
        if (!MolePuzzleManager.Instance.canChooseAnswer)
        {
            Debug.Log("⚠️ 아직 정답을 선택할 수 없습니다. 가이드의 세 번째 대사를 먼저 들어야 합니다.");
            return;
        }

        if (isAnswer)
        {
            Debug.Log("정답!");
            Debug.Log("🎯 정답입니다! 씬 전환합니다!");

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                Debug.Log($"▶ 씬 전환 시도: {nextSceneName}");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogWarning("⚠️ nextSceneName이 비어 있어서 씬 전환 실패");
            }
        }
        else
        {
            Debug.Log("❌ 오답입니다. 다시 선택해주세요.");
            isDialogueFinished = false;
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

