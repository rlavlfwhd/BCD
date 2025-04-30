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
    public Material insertedPendantMaterial;
    public Material openedDoorMaterial;
    public GameObject clickableDoorObject;
    public GameObject overlayImage;
    public string puzzleID = "SecretPath";
    public int nextStoryIndex = 301;

    private bool isPendantInserted = false;
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
        if (isDoorOpened)
        {
            StartCoroutine(GoToStoryAfterDelay(2f));
        }
        else if (isPendantInserted)
        {
            // 중간 상태에서 문 클릭 시 열림
            OpenDoorFully();
        }
        else
        {
            TryClick();
        }
    }

    void OpenDoorFully()
    {
        PuzzleManager.Instance.CompletePuzzleAndConsumeItem(puzzleID, neededItem);

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
        Debug.Log("문이 열렸습니다!");
    }

    void TryClick()
    {
        Item selected = Inventory.Instance.firstSelectedItem;

        if (selected != null && selected == neededItem && !PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            isPendantInserted = true;

            if (doorRenderer != null && insertedPendantMaterial != null)
            {
                doorRenderer.material = insertedPendantMaterial;
            }

            Debug.Log("펜던트가 문에 꽂혔습니다. 1초 후 문이 열립니다.");

            StartCoroutine(OpenDoorAfterDelay(1f)); // 1초 후 문 열림
        }
    }

    private IEnumerator OpenDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        PuzzleManager.Instance.CompletePuzzleAndConsumeItem(puzzleID, neededItem);

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
        Debug.Log("문이 열렸습니다!");
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
