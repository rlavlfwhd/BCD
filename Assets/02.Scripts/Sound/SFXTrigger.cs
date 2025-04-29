using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SFXTrigger : MonoBehaviour, IPointerClickHandler
{
    [Header("이 오브젝트에서 재생할 효과음")]
    public AudioClip sfxClip;

    [Header("SFX 볼륨 설정 (0 ~ 1)")]
    [Range(0f, 1f)]
    public float volume = 1.0f;

    public void PlaySFX()
    {
        if (sfxClip != null)
        {
            SoundManager.instance.PlaySFX(sfxClip, volume);
        }
        else
        {
            Debug.LogWarning("SFX Clip이 설정되지 않았습니다!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySFX();
    }
}