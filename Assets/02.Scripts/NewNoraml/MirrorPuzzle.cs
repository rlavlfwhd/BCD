using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MirrorPuzzle : MonoBehaviour, IClickablePuzzle
{
    public AudioClip mirrorBreakClip;
    public AudioMixerGroup sfxMixerGroup;

    public string puzzleID = "Mirror";
    public Item neededItem;          // 사용할 아이템 (ChickenStatue)
    public SpriteRenderer mirrorRenderer;
    public Sprite brokenMirrorSprite1; // 닭동상 사용 후 이미지
    public Sprite brokenMirrorSprite2; // 펜던트 지급 후 이미지

    public Item pendantItem;         // 지급할 아이템 (Pendant)
    public GameObject mirrorPanel;   // Pendant 지급 시 표시할 패널

    private bool isPuzzleCompleted = false;
    private bool isItemGiven = false;

    void OnEnable()
    {
        StartCoroutine(InitializePuzzleState());
    }

    IEnumerator InitializePuzzleState()
    {
        yield return new WaitUntil(() => PuzzleManager.Instance != null);
        yield return null;

        if (!PuzzleManager.Instance.IsPuzzleCompleted(puzzleID)) yield break;

        isPuzzleCompleted = true;

        if (SceneDataManager.Instance.Data.mirrorItemGiven.Contains(puzzleID))
        {
            isItemGiven = true;

            if (mirrorRenderer != null && brokenMirrorSprite2 != null)
            {
                mirrorRenderer.sprite = brokenMirrorSprite2;
            }
        }
        else
        {
            isItemGiven = false;

            if (mirrorRenderer != null && brokenMirrorSprite1 != null)
            {
                mirrorRenderer.sprite = brokenMirrorSprite1;
            }
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

            if (mirrorRenderer != null && brokenMirrorSprite1 != null)
            {
                mirrorRenderer.sprite = brokenMirrorSprite1;
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

            if (!SceneDataManager.Instance.Data.mirrorItemGiven.Contains(puzzleID))
            {
                SceneDataManager.Instance.Data.mirrorItemGiven.Add(puzzleID);
            }

            Debug.Log("Pendant 아이템 지급 완료!");

            if (mirrorRenderer != null && brokenMirrorSprite2 != null)
            {
                mirrorRenderer.sprite = brokenMirrorSprite2;
                Debug.Log("거울 최종 상태로 변경 완료!");
            }
        }
    }
}