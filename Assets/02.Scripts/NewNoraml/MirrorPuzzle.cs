using System.Collections;
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
    public SpriteRenderer mirrorRenderer;
    public Sprite brokenMirrorSprite1;
    public Sprite brokenMirrorSprite2;

    public Item pendantItem;         // 지급할 아이템 (Pendant)
    public GameObject mirrorPanel;   // Pendant 지급 시 표시할 패널

    private bool isPuzzleCompleted = false;
    private bool isItemGiven = false;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PuzzleManager.Instance != null);
        yield return null; // 퍼즐 완료 상태가 복원된 뒤 1프레임 대기

        if (!PuzzleManager.Instance.IsPuzzleCompleted(puzzleID)) yield break;

        isPuzzleCompleted = true;
        isItemGiven = true;

        if (mirrorRenderer != null && brokenMirrorSprite2 != null)
        {
            mirrorRenderer.sprite = brokenMirrorSprite2;
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
                SoundManager.PlayOneShot(gameObject, mirrorBreakClip, sfxMixerGroup);
            }

            isPuzzleCompleted = true;
        }
    }

    void GivePendant()
    {
        if (!isItemGiven)
        {
            Inventory.Instance.AddItem(pendantItem);
            isItemGiven = true;
            Debug.Log("Pendant 아이템 지급 완료!");

            if (mirrorRenderer != null && brokenMirrorSprite2 != null)
            {
                mirrorRenderer.sprite = brokenMirrorSprite2;
            }
        }
    }
}
