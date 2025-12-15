using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class ShakeController : MonoBehaviour
{
    [Header("💫 흔들림 설정")]
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.1f;
    public float shakeSpeed = 20f;
    public float tiltAngle = 10f;

    [Header("📈 위로 이동 설정")]
    public float riseHeight = 0.5f;
    public float riseSpeed = 3f;

    [Header("🍷 따를 잔 위치")]
    public Transform glassTarget;

    [Header("🍾 따르기 연출 설정")]
    public GameObject pourEffect;
    public float pourDuration = 1f;
    public float moveDuration = 1f;
    public float pourTiltAngle = -30f;

    [Header("🍷 잔 컨트롤러")]
    public WineGlassController wineGlassController;

    // 🔊 쉐이크 사운드
    [Header("🔊 쉐이크 사운드")]
    public AudioClip shakeSound;
    public AudioMixerGroup sfxMixerGroup;

    // 🔊 [추가] 따르기 사운드
    [Header("🔊 따르기 사운드")]
    public AudioClip pourSound;


    private AudioSource audioSource;
    private AudioSource pourAudioSource; // ← 추가

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        if (pourEffect != null)
            pourEffect.SetActive(false);

        // 🔊 쉐이크 AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;

        // 🔊 [추가] 따르기 AudioSource
        pourAudioSource = gameObject.AddComponent<AudioSource>();
        pourAudioSource.playOnAwake = false;
        pourAudioSource.loop = true;
        pourAudioSource.outputAudioMixerGroup = sfxMixerGroup;
    }

    public IEnumerator StartShaking(bool isSuccess)
    {
        yield return StartCoroutine(ShakeRoutine(isSuccess));
    }

    private IEnumerator ShakeRoutine(bool isSuccess)
    {
        float elapsed = 0f;
        Vector3 targetPos = originalPosition + Vector3.up * riseHeight;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * riseSpeed);
            yield return null;
        }

        // 🔊 쉐이크 사운드 시작
        if (shakeSound != null)
        {
            audioSource.clip = shakeSound;
            audioSource.Play();
        }

        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            float xOffset = Mathf.Sin(elapsed * shakeSpeed) * shakeMagnitude;
            float zRotation = Mathf.Sin(elapsed * shakeSpeed) * tiltAngle;

            transform.position = targetPos + new Vector3(xOffset, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, zRotation);

            yield return null;
        }

        if (audioSource.isPlaying)
            audioSource.Stop();

        transform.rotation = originalRotation;

        yield return StartCoroutine(MoveAndPourRoutine(isSuccess));
    }

    private IEnumerator MoveAndPourRoutine(bool isSuccess)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = glassTarget.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / moveDuration);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = Quaternion.Euler(0, 0, pourTiltAngle);

        // 🍾 따르기 이펙트
        if (pourEffect != null)
            pourEffect.SetActive(true);

        // 🔊 [추가] 따르기 사운드 시작
        if (pourSound != null)
        {
            pourAudioSource.clip = pourSound;
            pourAudioSource.Play();
        }

        if (wineGlassController != null)
        {
            if (isSuccess)
                wineGlassController.StartFadeInFilledGlass();
            else
                wineGlassController.ShowWeirdWine();
        }

        yield return new WaitForSeconds(pourDuration);

        // 🔊 [추가] 따르기 사운드 종료
        if (pourAudioSource.isPlaying)
            pourAudioSource.Stop();

        if (pourEffect != null)
            pourEffect.SetActive(false);

        transform.rotation = originalRotation;

        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(targetPos, originalPosition, elapsed / moveDuration);
            yield return null;
        }

        transform.position = originalPosition;
    }
}
