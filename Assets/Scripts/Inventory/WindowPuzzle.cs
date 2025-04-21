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
    public CameraParallax cameraParallax;

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

            Debug.Log("3D â�� ����! Rope4 ��� �Ϸ�");

            StartCoroutine(GoToStoryAfterDelay(2f));
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        if (overlayImage != null)
        {
            overlayImage.SetActive(true); // �������� ǥ��
            cameraParallax.ResetToInitialPosition();
            cameraParallax.enabled = false;
        }

        yield return new WaitForSeconds(delay);
        cameraParallax.enabled = true;
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("PlayScene");
    }
}

