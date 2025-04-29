using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    public Item item;
    public AudioClip itemClickClip;
    public AudioMixerGroup sfxMixerGroup;

    public Item ClickItem()
    {
        SoundManager.PlayOneShot(gameObject, itemClickClip, sfxMixerGroup);

        return this.item;
    }
}