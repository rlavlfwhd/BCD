using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MirrorPuzzle : MonoBehaviour
{
    public string puzzleID = "MirrorPuzzle";
    public Item neededItem;          // 사용할 아이템 (ChickenStatue)
    public MeshRenderer mirrorRenderer;
    public Material brokenMirrorMaterial;

    public Item pendantItem;         // 지급할 아이템 (Pendant)
    public GameObject mirrorPanel;   // Pendant 지급 시 표시할 패널

    [Header("사운드 매니저에 등록된 이름")]
    public string mirrorBreakSoundName; // ✅ 거울이 깨질 때 재생할 사운드 이름

    private bool isPuzzleCompleted = false;
    private bool isItemGiven = false;

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

                // ✅ 거울 변경할 때 사운드 재생
                if (!string.IsNullOrEmpty(mirrorBreakSoundName))
                {
                    SoundManager.instance.PlaySound(mirrorBreakSoundName);
                }
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
        else
        {
            if (mirrorPanel != null)
            {
                mirrorPanel.SetActive(false);
            }
            Debug.Log("이미 Pendant를 획득했습니다!");
        }
    }
}
