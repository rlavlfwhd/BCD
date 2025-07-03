using UnityEngine;
using UnityEngine.Audio;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    public Item item;

    [Header("🎵 아이템 클릭 사운드")]
    public AudioClip itemClickClip;
    public AudioMixerGroup sfxMixerGroup;

    [Header("🔽 클릭 시 보여줄 화살표")]
    [SerializeField] private GameObject arrowObject;

    public Item ClickItem()
    {
        Debug.Log($"[클릭됨] 아이템: {item?.name}, 사운드 클립: {(itemClickClip != null ? itemClickClip.name : "없음")}");

        // ✅ 사운드 재생 (무조건 재생됨)
        SoundManager.PlayOneShot(itemClickClip, sfxMixerGroup);

        // ✅ 화살표 오브젝트 활성화
        if (arrowObject != null)
        {
            arrowObject.SetActive(true);
            Debug.Log("화살표가 활성화되었습니다!");
        }
        else
        {
            Debug.LogWarning("arrowObject가 할당되지 않았습니다!");
        }

        return item;
    }
}
