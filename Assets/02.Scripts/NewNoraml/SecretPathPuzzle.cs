using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SecretPath : MonoBehaviour
{
    public Item neededItem; // Pendant
    public MeshRenderer DoorRenderer;
    public Material openedDoorMaterial;
    public GameObject clickableWindowObject;
    public string puzzleID = "SecretPath";
    public int nextStoryIndex = 301;

    [Header("사운드 매니저에 등록된 이름")] // ✅ 추가
    public string doorOpenSoundName; // ✅ 문 열 때 재생할 사운드 이름

    private bool isDoorOpened = false;

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

            if (DoorRenderer != null && openedDoorMaterial != null)
            {
                DoorRenderer.material = openedDoorMaterial;
            }

            if (clickableWindowObject != null)
            {
                clickableWindowObject.SetActive(true);
            }

            isDoorOpened = true;
            Debug.Log("철문열림! 펜던트 사용 완료");

            // ✅ 문 열릴 때 사운드 재생
            if (!string.IsNullOrEmpty(doorOpenSoundName))
            {
                SoundManager.instance.PlaySound(doorOpenSoundName);
            }
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }
}
