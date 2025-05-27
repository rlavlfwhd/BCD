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
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 말풍선을 지정된 위치(anchor)에 생성하고 메시지를 출력
    /// </summary>
    public void ShowBubble(MoleController caller, Transform anchor, string message)
    {
        Debug.Log($"🧪 ShowBubble 호출됨 by {caller.name}, 메시지: {message}");

        if (speechBubblePrefab == null)
        {
            Debug.LogError("❌ 말풍선 프리팹이 연결되지 않았습니다.");
            return;
        }

        if (anchor == null)
        {
            Debug.LogError($"❌ {caller.name}의 말풍선 위치 anchor(Transform)가 지정되지 않았습니다.");
            return;
        }

        if (currentBubble != null)
        {
            Destroy(currentBubble);
        }

        // 말풍선 생성
        Vector3 spawnPos = anchor.position;
        currentBubble = Instantiate(speechBubblePrefab, spawnPos, Quaternion.identity);

        // ✅ 먼저 부모 연결
        currentBubble.transform.SetParent(anchor, worldPositionStays: true);
        currentBubble.transform.localScale = Vector3.one;

        // ✅ 회전은 반드시 SetParent 이후에 실행 (기울어짐 방지)
        Transform cam = Camera.main?.transform;
        if (cam != null)
        {
            Vector3 direction = currentBubble.transform.position - cam.position;
            direction.y = 0f;
        }

        // Canvas 카메라 연결
        Canvas canvas = currentBubble.GetComponentInChildren<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }

        // 텍스트 타이핑 출력
        TextMeshProUGUI text = currentBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            if (typingRoutine != null)
                StopCoroutine(typingRoutine);

            typingRoutine = StartCoroutine(TypeText(caller, text, message));
        }
        else
        {
            Debug.LogWarning("⚠️ 말풍선 안에 TextMeshProUGUI 컴포넌트가 없습니다.");
        }
    }

    private IEnumerator TypeText(MoleController caller, TextMeshProUGUI text, string message)
    {
        text.text = "";
        foreach (char c in message)
        {
            text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        caller.OnDialogueComplete();
    }
}



