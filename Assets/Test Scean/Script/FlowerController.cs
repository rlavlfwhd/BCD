using UnityEngine;

public class FlowerController : MonoBehaviour, IClickablePuzzle
{
    [Header("🌸 꽃 스프라이트 설정")]
    public SpriteRenderer flowerSprite;          // 꽃 이미지를 표시하는 SpriteRenderer
    public Sprite[] flowerSprites;                // 꽃잎이 줄어들 때마다 교체할 스프라이트 배열

    [Header("🌿 꽃잎 수")]
    public int currentPetalCount = 4;              // 현재 남아 있는 꽃잎 수 (게임 시작 시 기본값)

    [Header("🧚 도착 지점")]
    public Transform arrivalPoint;                // 페어리가 이동할 때 도착할 정확한 위치

    [Header("🌸 스프라이트 페이드 옵션")]
    public float fadeDuration = 0.2f;              // 스프라이트가 부드럽게 전환될 때 걸리는 시간 (초)

    [Header("🍃 떨어지는 꽃잎 프리팹")]
    public GameObject petalPrefab;                 // 떨어질 꽃잎(떨어지는 이펙트용 프리팹)
    public Vector3 petalSpawnOffset = Vector3.zero; // 꽃잎 생성 시 꽃 위치에서 얼마나 Offset을 줄지 설정 (보통 약간 위로)

    private Coroutine fadeCoroutine;               // 현재 실행 중인 페이드 코루틴 (중복 방지용)

    private FlowerPuzzleController flowerPuzzleController;

    private void Start()
    {
        flowerPuzzleController = FindObjectOfType<FlowerPuzzleController>();
    }
    /// <summary>
    /// 꽃잎을 하나 떨어뜨리는 함수 (페이드 전환 + 꽃잎 이펙트 생성)
    /// </summary>
    public void DropPetal()
    {
        if (currentPetalCount > 0) // 아직 꽃잎이 남아 있다면
        {
            currentPetalCount--; // 꽃잎 수 하나 감소
            Debug.Log($"{gameObject.name} 꽃잎 하나 떨어짐! 남은 꽃잎 수: {currentPetalCount}");

            // 스프라이트 변경
            int spriteIndex = Mathf.Clamp(currentPetalCount, 0, flowerSprites.Length - 1); // 배열 범위 초과 방지
            Sprite newSprite = flowerSprites[spriteIndex];

            StartFadeToNewSprite(newSprite); // 부드럽게 새 스프라이트로 전환 시작

            // 떨어지는 꽃잎 프리팹 생성
            if (petalPrefab != null)
            {
                Vector3 spawnPos = transform.position + petalSpawnOffset; // Offset을 적용한 위치
                Instantiate(petalPrefab, spawnPos, Quaternion.identity); // 꽃잎 프리팹 생성
            }
        }
    }

    /// <summary>
    /// 새 스프라이트로 부드럽게 교체하는 코루틴 시작
    /// </summary>
    private void StartFadeToNewSprite(Sprite newSprite)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine); // 기존 코루틴 중복 방지
        fadeCoroutine = StartCoroutine(FadeToNewSprite(newSprite)); // 새 코루틴 실행
    }

    /// <summary>
    /// 스프라이트를 자연스럽게 사라졌다가 다시 나타나는 방식으로 교체하는 코루틴
    /// </summary>
    private System.Collections.IEnumerator FadeToNewSprite(Sprite newSprite)
    {
        // 1단계: 현재 스프라이트를 점점 투명하게 만들기
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float a = 1f - (t / fadeDuration); // 1 → 0 으로 감소
            flowerSprite.color = new Color(1f, 1f, 1f, a); // 알파(투명도) 조절
            yield return null; // 한 프레임 대기
        }

        // 스프라이트 교체
        flowerSprite.sprite = newSprite;

        // 2단계: 새 스프라이트를 점점 불투명하게 만들기
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float a = t / fadeDuration; // 0 → 1 로 증가
            flowerSprite.color = new Color(1f, 1f, 1f, a); // 알파(투명도) 조절
            yield return null; // 한 프레임 대기
        }

        // 마지막에는 완전히 불투명하게 고정
        flowerSprite.color = Color.white;
    }
    public void OnClickPuzzle()
    {
        if (flowerPuzzleController == null)
            return;

        // 퍼즐 클리어 상태 또는 페어리 이동 중일 때 클릭 막기
        if (flowerPuzzleController.IsPuzzleCleared() || flowerPuzzleController.IsFairyMoving())
            return;

        flowerPuzzleController.OnFlowerClicked(this);
    }
}




