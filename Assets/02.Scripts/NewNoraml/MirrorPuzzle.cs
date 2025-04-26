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

    private bool isPuzzleCompleted = false; // 퍼즐 완료 여부
    private bool isItemGiven = false;       // Pendant 지급 여부

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
                mirrorPanel.SetActive(true); // Pendant 얻으면 패널 열기
            }
        }
        else
        {
            if (mirrorPanel != null)
            {
                mirrorPanel.SetActive(false); // 이미 얻었으면 패널 닫기
            }
            Debug.Log("이미 Pendant를 획득했습니다!");
        }
    }
}