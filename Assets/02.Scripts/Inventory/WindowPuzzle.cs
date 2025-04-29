using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowPuzzle : MonoBehaviour
{
    public AudioClip openWindowClip;
    public AudioMixerGroup sfxMixerGroup;

    public Item neededItem; // Rope4
    public MeshRenderer windowRenderer;
    public Material openedWindowMaterial;
    public GameObject clickableWindowObject;
    public GameObject overlayImage;
    public string puzzleID = "window_rope";
    public int nextStoryIndex = 200;

    private bool isWindowOpened = false;

    void Start()
    {
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            if (windowRenderer != null && openedWindowMaterial != null)
            {
                windowRenderer.material = openedWindowMaterial;
            }

            if (clickableWindowObject != null)
            {
                clickableWindowObject.SetActive(true);
            }

            isWindowOpened = true;

            var myObjectItem = GetComponent<IObjectItem>();
            if (myObjectItem != null)
            {
                gameObject.SetActive(false);
            }
        }
    }


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

        if (selected != null && selected == neededItem && !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            PuzzleManager.Instance.CompletePuzzleAndConsumeItem(puzzleID, selected);

            if (windowRenderer != null && openedWindowMaterial != null)
            {
                windowRenderer.material = openedWindowMaterial;

                SoundManager.PlayOneShot(gameObject, openWindowClip, sfxMixerGroup);
            }

            if (clickableWindowObject != null)
            {
                clickableWindowObject.SetActive(true);
            }

            isWindowOpened = true;

            Debug.Log("3D 창문 열림! Rope4 사용 완료");
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

            Material mat = renderer.material;
            Color color = mat.color;

            color.a = 0f;
            mat.color = color;

            float timer = 0f;
            float fadeDuration = 1f;

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

        yield return new WaitForSeconds(delay);

        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }
}
