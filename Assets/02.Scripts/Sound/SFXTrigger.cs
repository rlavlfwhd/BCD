using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SFXTrigger : MonoBehaviour, IPointerClickHandler
{
    [Header("�� ������Ʈ���� ����� ȿ����")]
    public AudioClip sfxClip;

    [Header("SFX ���� ���� (0 ~ 1)")]
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
            Debug.LogWarning("SFX Clip�� �������� �ʾҽ��ϴ�!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySFX();
    }
}