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

    [Header("❌ 실패 처리 매니저")]
    public MolePuzzleFailManager puzzleManager;

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
        // ✅ 가이드 또는 일반 두더지 클릭 → 대사 출력
        MolePuzzleManager.Instance.SelectMole(this); // 크기 조절

        ShowDialogue(); // 대사 출력

        // ✅ 가이드는 클릭 시 항상 대사만 출력 (정답 체크 안 함)
        if (isGuide) return;

        // ✅ 일반 두더지일 경우, 정답 선택 가능 상태일 때 바로 정답/오답 판정
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

        // ✅ 가이드의 3번째 대사 출력 시 정답 선택 가능 상태로 전환
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
            Debug.Log("❌ 오답입니다. 퍼즐 실패 처리!");
            isDialogueFinished = false;

            if (puzzleManager != null)
            {
                puzzleManager.HandleFail(); // 페이드 아웃 → 재시작
            }
            else
            {
                Debug.LogWarning("⚠️ puzzleManager가 연결되지 않았습니다!");
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


