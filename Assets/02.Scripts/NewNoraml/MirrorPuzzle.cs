using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MirrorPuzzle : MonoBehaviour
{
    public AudioClip mirrorBreakClip;
    public AudioMixerGroup sfxMixerGroup;

    public string puzzleID = "MirrorPuzzle";
    public Item neededItem;          // 사용할 아이템 (ChickenStatue)
    public MeshRenderer mirrorRenderer;
    public Material brokenMirrorMaterial;

    public Item pendantItem;         // 지급할 아이템 (Pendant)
    public GameObject mirrorPanel;   // Pendant 지급 시 표시할 패널

    private bool isPuzzleCompleted = false;
    private bool isItemGiven = false;

    private void Start()
    {
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            // 거울이 깨진 상태로 복구
            if (mirrorRenderer != null && brokenMirrorMaterial != null)
            {
                mirrorRenderer.material = brokenMirrorMaterial;
            }

            isPuzzleCompleted = true;
            isItemGiven = true;
        }
    }

    void OnMouseDown()
    {
        if (!isPuzzleCompleted)
        {
            TryUseItem();
        }
        else
        {
            GivePendant();
        }
    }

    void TryUseItem()
    {
        Item selected = Inventory.Instance.firstSelectedItem;

        if (selected != null && selected == neededItem && !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            PuzzleManager.Instance.CompletePuzzleAndConsumeItem(puzzleID, selected);

            if (mirrorRenderer != null && brokenMirrorMaterial != null)
            {
                mirrorRenderer.material = brokenMirrorMaterial;
                Debug.Log("거울 머테리얼 변경 완료!");

                SoundManager.PlayOneShot(gameObject, mirrorBreakClip, sfxMixerGroup);
            }

            isPuzzleCompleted = true;
            Debug.Log("거울 퍼즐 완료! 닭동상 사용됨");
        }
    }

    void GivePendant()
    {
        if (!isItemGiven)
        {
            Inventory.Instance.AddItem(pendantItem);
            isItemGiven = true;
            Debug.Log("Pendant 아이템 획득!");

            if (mirrorPanel != null)
            {
                mirrorPanel.SetActive(true);
            }
        }
    }
}
