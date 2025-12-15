using UnityEngine;
using UnityEngine.Audio;

public class WinePourLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform startPoint;   // 와인병 위치
    public Transform endPoint;     // 쉐이커 위치
    public float pourSpeed = 2f;   // 내려오는 속도

    private float currentLength = 0f;

    [Header("?? 와인 붓는 사운드")]
    public AudioClip pourSound;
    public AudioMixerGroup sfxMixerGroup;

    private bool isSoundPlayed = false;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;

        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, startPoint.position);
    }

    void Update()
    {
        if (currentLength < 1f)
        {
            currentLength += Time.deltaTime * pourSpeed;
            Vector3 targetPos = Vector3.Lerp(startPoint.position, endPoint.position, currentLength);
            lineRenderer.SetPosition(1, targetPos);

            // ?? SFX 사운드 처음 한 번만 재생
            if (!isSoundPlayed && pourSound != null && sfxMixerGroup != null)
            {
                AudioSource sfx = gameObject.AddComponent<AudioSource>();
                sfx.clip = pourSound;
                sfx.outputAudioMixerGroup = sfxMixerGroup;
                sfx.playOnAwake = false;
                sfx.volume = 1f;
                sfx.Play();
                Destroy(sfx, pourSound.length + 0.1f);

                isSoundPlayed = true;
                Debug.Log("[SFX] 와인 붓기 사운드 재생됨 (Mixer 연결)");
            }
        }
    }
}
