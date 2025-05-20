using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SecretPath : MonoBehaviour, IClickablePuzzle
{
    public AudioClip openDoorClip;
    public AudioMixerGroup sfxMixerGroup;

    public Item neededItem; // Pendant
    public SpriteRenderer doorRenderer;
    public Sprite insertedPendantSprite;
    public Sprite openedDoorSprite;
    public GameObject clickableDoorObject;
    public GameObject overlayImage;
    public string puzzleID = "SecretPath";
    public int nextStoryIndex = 301;

    private bool isPendantInserted = false;
    private bool isDoorOpened = false;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => PuzzleManager.Instance != null);
        yield return null;

        if (!PuzzleManager.Instance.IsPuzzleCompleted(puzzleID)) yield break;

        if (doorRenderer != null && openedDoorSprite != null)
        {
            doorRenderer.sprite = openedDoorSprite;
        }

        if (clickableDoorObject != null)
        {
            clickableDoorObject.SetActive(true);
        }

        isDoorOpened = true;
    }

    public void OnClickPuzzle()
    {
        if (isDoorOpened)
        {
            StartCoroutine(GoToStoryAfterDelay(2f));
        }
        else if (isPendantInserted)
        {
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

        if (doorRenderer != null && openedDoorSprite != null)
        {
            doorRenderer.sprite = openedDoorSprite;
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

            if (doorRenderer != null && insertedPendantSprite != null)
            {
                doorRenderer.sprite = openedDoorSprite;
            }

            Debug.Log("펜던트가 문에 꽂혔습니다. 1초 후 문이 열립니다.");

            StartCoroutine(OpenDoorAfterDelay(1f)); // 1초 후 문 열림
        }
    }

    private IEnumerator OpenDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        PuzzleManager.Instance.CompletePuzzleAndConsumeItem(puzzleID, neededItem);

        if (doorRenderer != null && openedDoorSprite != null)
        {
            doorRenderer.sprite = openedDoorSprite;
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
