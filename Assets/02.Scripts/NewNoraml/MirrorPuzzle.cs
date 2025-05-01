using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MirrorPuzzle : MonoBehaviour, IClickablePuzzle
{
    public AudioClip mirrorBreakClip;
    public AudioMixerGroup sfxMixerGroup;

    public string puzzleID = "MirrorPuzzle";
    public Item neededItem;          // 사용할 아이템 (ChickenStatue)
    public MeshRenderer mirrorRenderer;
    public Material brokenMirrorMaterial1;
    public Material brokenMirrorMaterial2;

    public Item pendantItem;         // 지급할 아이템 (Pendant)
    public GameObject mirrorPanel;   // Pendant 지급 시 표시할 패널

    private bool isPuzzleCompleted = false;
    private bool isItemGiven = false;

    private void Start()
    {
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            // 거울 상태 복원
            if (mirrorRenderer != null && brokenMirrorMaterial1 != null)
            {
                mirrorRenderer.material = brokenMirrorMaterial1;
            }

            isPuzzleCompleted = true;
            isItemGiven = true;
        }
    }

    public void OnClickPuzzle()
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

            if (mirrorRenderer != null && brokenMirrorMaterial1 != null)
            {
                mirrorRenderer.material = brokenMirrorMaterial1;
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
            Debug.Log("Pendant 아이템 지급 완료!");

            if (mirrorRenderer != null && brokenMirrorMaterial2 != null)
            {
                mirrorRenderer.material = brokenMirrorMaterial2;
                Debug.Log("거울 최종 상태로 변경 완료!");
            }
        }
    }
}
