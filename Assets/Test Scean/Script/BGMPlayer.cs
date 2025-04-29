using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private static BGMPlayer instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않음
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // 기본 설정
            audioSource.playOnAwake = true;
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject); // 이미 존재하면 새로 생성된 건 삭제
        }
    }

    // 외부에서 BGM을 재생하고 싶을 때 사용
    public void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    // 외부에서 BGM을 멈추고 싶을 때 사용
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
