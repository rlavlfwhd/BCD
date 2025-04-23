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
    public string puzzleID = "Pendant";
    public int nextStoryIndex = 301;

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

        if (selected != null &&
            selected == neededItem &&
            !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            PuzzleManager.Instance.CompletePuzzle(puzzleID);

            if (DoorRenderer != null && openedDoorMaterial != null)
            {
                DoorRenderer.material = openedDoorMaterial;
            }

            if (clickableWindowObject != null)
            {
                clickableWindowObject.SetActive(true);
            }

            Inventory.Instance.RemoveItemByName(selected.itemName);
            Inventory.Instance.ClearSelection();

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