using UnityEngine;
using TMPro;
using System.Collections;

public class SpeechBubbleManager : MonoBehaviour
{
    public static SpeechBubbleManager Instance;

    [Header("💬 말풍선 프리팹")]
    public GameObject speechBubblePrefab;

    [Header("⌨️ 타이핑 속도 (초당 글자 수 기준)")]
    public float typingSpeed = 0.05f;

    private GameObject currentBubble;
    private Coroutine typingRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("✅ SpeechBubbleManager 인스턴스 생성 완료");
        }
        else
        {
            Debug.LogWarning("⚠️ 중복된 SpeechBubbleManager 인스턴스 발견. 기존 인스턴스 유지됨");
            Destroy(gameObject);
        }
    }

    public void ShowBubble(Transform target, string message, Vector3 offset)
    {
        Debug.Log($"🧪 [ShowBubble 호출됨] 대상: {target.name}, 대사: {message}");

        if (speechBubblePrefab == null)
        {
            Debug.LogError("❌ ShowBubble 실패: speechBubblePrefab이 설정되지 않았습니다!");
            return;
        }

        if (currentBubble != null)
        {
            Debug.Log("🔁 기존 말풍선 제거");
            Destroy(currentBubble);
        }

        Vector3 spawnPos = target.position + offset;
        currentBubble = Instantiate(speechBubblePrefab, spawnPos, Quaternion.identity);
        currentBubble.transform.SetParent(target, worldPositionStays: true);

        // 수평으로만 카메라 바라보게 설정
        Transform cam = Camera.main.transform;
        Vector3 lookPos = cam.position - currentBubble.transform.position;
        lookPos.y = 0f;
        if (lookPos.sqrMagnitude > 0.01f)
        {
            currentBubble.transform.rotation = Quaternion.LookRotation(lookPos);
            currentBubble.transform.Rotate(0f, 180f, 0f);
        }

        Canvas canvas = currentBubble.GetComponentInChildren<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
            Debug.Log("📷 Canvas에 메인 카메라 자동 연결 완료");
        }

        TextMeshProUGUI text = currentBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            // 코루틴으로 타이핑 효과 시작
            if (typingRoutine != null)
                StopCoroutine(typingRoutine);

            typingRoutine = StartCoroutine(TypeText(text, message));
        }
        else
        {
            Debug.LogWarning("⚠️ 말풍선 프리팹에 TextMeshProUGUI 컴포넌트가 없습니다.");
        }
    }

    // 🧵 타이핑 효과 코루틴
    private IEnumerator TypeText(TextMeshProUGUI text, string message)
    {
        text.text = ""; // 초기화
        foreach (char c in message)
        {
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
