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
            DontDestroyOnLoad(gameObject); // ���� �ٲ� �ı����� ����
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // �⺻ ����
            audioSource.playOnAwake = true;
            audioSource.loop = true;
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϸ� ���� ������ �� ����
        }
    }

    // �ܺο��� BGM�� ����ϰ� ���� �� ���
    public void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    // �ܺο��� BGM�� ���߰� ���� �� ���
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
