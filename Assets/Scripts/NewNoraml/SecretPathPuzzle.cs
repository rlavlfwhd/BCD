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
    public int nextStoryIndex = 3;

    void OnMouseDown()
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

            Debug.Log("3D 창문 열림! Rope4 사용 완료");

            //StartCoroutine(GoToStoryAfterDelay(2f));
        }
    }

    /*private IEnumerator GoToStoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("PlayScene");
    }*/
}