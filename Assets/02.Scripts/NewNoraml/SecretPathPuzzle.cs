using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SecretPath : MonoBehaviour
{
    public AudioClip openDoorClip;
    public AudioMixerGroup sfxMixerGroup;

    public Item neededItem; // Pendant
    public MeshRenderer doorRenderer;
    public Material openedDoorMaterial;
    public GameObject clickableDoorObject;
    public string puzzleID = "SecretPath";
    public int nextStoryIndex = 301;

    private bool isDoorOpened = false;

    void Start()
    {
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            // 문 열린 상태로 복원
            if (doorRenderer != null && openedDoorMaterial != null)
            {
                doorRenderer.material = openedDoorMaterial;
            }

            if (clickableDoorObject != null)
            {
                clickableDoorObject.SetActive(true);
            }

            isDoorOpened = true;
        }
    }
    private void OnMouseDown()
    {
        if (!isDoorOpened)
        {
            TryClick();
        }
        else
        {
            StartCoroutine(GoToStoryAfterDelay(2f));
        }
    }

    void TryClick()
    {
        Item selected = Inventory.Instance.firstSelectedItem;

        if (selected != null && selected == neededItem && !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            PuzzleManager.Instance.CompletePuzzleAndConsumeItem(puzzleID, selected);

            if (doorRenderer != null && openedDoorMaterial != null)
            {
                doorRenderer.material = openedDoorMaterial;

                SoundManager.PlayOneShot(gameObject, openDoorClip, sfxMixerGroup);
            }

            if (clickableDoorObject != null)
            {
                clickableDoorObject.SetActive(true);
            }

            isDoorOpened = true;
            Debug.Log("철문열림! 펜던트 사용 완료");
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }
}
