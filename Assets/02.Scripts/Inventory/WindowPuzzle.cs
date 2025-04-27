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
    public GameObject overlayImage;
    public string puzzleID = "window_rope";
    public int nextStoryIndex = 11;

    private bool isWindowOpened = false;

    private void OnMouseDown()
    {
        if (!isWindowOpened)
        {
            TryUseRope();
        }
        else
        {
            StartCoroutine(GoToStoryAfterDelay(2f));
        }
    }

    void TryUseRope()
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

            isWindowOpened = true;

            Debug.Log("3D â�� ����! Rope4 ��� �Ϸ�");
        }
    }

    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        if (overlayImage != null)
        {
            overlayImage.SetActive(true);

            Renderer renderer = overlayImage.GetComponent<Renderer>();
            if (renderer == null)
                renderer = overlayImage.GetComponentInChildren<Renderer>();

            Material mat = renderer.material; // material �ν��Ͻ�
            Color color = mat.color;

            // �ʱ� ���� ���� (���� ����)
            color.a = 0f;
            mat.color = color;

            // ���̵� �� (���� �� ������)
            float timer = 0f;
            float fadeDuration = 1; // ��Ÿ���� �� �ɸ��� �ð�

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                mat.color = color;
                yield return null;
            }
            color.a = 1f;
            mat.color = color;
        }

        // ��ٸ��� �ð� (��: 2��)
        yield return new WaitForSeconds(delay);

        // ���丮������ ��ȯ
        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }
}
