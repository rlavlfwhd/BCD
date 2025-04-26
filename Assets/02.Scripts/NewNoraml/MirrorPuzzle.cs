using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MirrorPuzzle : MonoBehaviour
{
    public string puzzleID = "MirrorPuzzle";
    public Item neededItem;          // ����� ������ (ChickenStatue)
    public MeshRenderer mirrorRenderer;
    public Material brokenMirrorMaterial;
    

    public Item pendantItem;         // ������ ������ (Pendant)
    public GameObject mirrorPanel;   // Pendant ���� �� ǥ���� �г�

    private bool isPuzzleCompleted = false; // ���� �Ϸ� ����
    private bool isItemGiven = false;       // Pendant ���� ����

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
                Debug.Log("�ſ� ���׸��� ���� �Ϸ�!");
            }

            isPuzzleCompleted = true;
            Debug.Log("�ſ� ���� �Ϸ�! �ߵ��� ����");
        }
    }

    void GivePendant()
    {
        if (!isItemGiven)
        {
            Inventory.Instance.AddItem(pendantItem);
            isItemGiven = true;
            Debug.Log("Pendant ������ ȹ��!");

            if (mirrorPanel != null)
            {
                mirrorPanel.SetActive(true); // Pendant ������ �г� ����
            }
        }
        else
        {
            if (mirrorPanel != null)
            {
                mirrorPanel.SetActive(false); // �̹� ������� �г� �ݱ�
            }
            Debug.Log("�̹� Pendant�� ȹ���߽��ϴ�!");
        }
    }
}