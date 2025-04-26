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
            Debug.Log("ö������! ���Ʈ ��� �Ϸ�");            
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }
}