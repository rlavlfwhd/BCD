using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowPuzzle : MonoBehaviour
{
    public Item neededItem; // Rope4
    public MeshRenderer windowRenderer;
    public Material openedWindowMaterial;
    public GameObject clickableWindowObject;
    public GameObject overlayImage; // 
    public string puzzleID = "window_rope";
    public int nextStoryIndex = 11;

    void OnMouseDown()
    {
        Item selected = Inventory.Instance.firstSelectedItem;

        if (selected != null &&
            selected == neededItem &&
            !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            PuzzleManager.Instance.CompletePuzzle(puzzleID);

            if (windowRenderer != null && openedWindowMaterial != null)
            {
                windowRenderer.material = openedWindowMaterial;
            }

            if (clickableWindowObject != null)
            {
                clickableWindowObject.SetActive(true);
            }

            Inventory.Instance.RemoveItemByName(selected.itemName);
            Inventory.Instance.ClearSelection();

            Debug.Log("3D 창문 열림! Rope4 사용 완료");

            StartCoroutine(GoToStoryAfterDelay(2f));
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        if (overlayImage != null)
        {
            overlayImage.SetActive(true); // 오버레이 표시            
        }

        yield return new WaitForSeconds(delay);
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("PlayScene");
    }
}

