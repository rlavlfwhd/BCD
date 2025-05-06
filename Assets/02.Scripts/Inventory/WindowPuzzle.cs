using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WindowPuzzle : MonoBehaviour, IClickablePuzzle
{
    public AudioClip openWindowClip;
    public AudioMixerGroup sfxMixerGroup;

    public Item neededItem; // Rope4
    public SpriteRenderer windowRenderer;
    public Sprite openedWindowSprite;
    public GameObject clickableWindowObject;
    public GameObject overlayImage;
    public string puzzleID = "window_rope";
    public int nextStoryIndex = 200;

    private bool isWindowOpened = false;

    void Start()
    {
        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            if (windowRenderer != null && openedWindowSprite != null)
            {
                windowRenderer.sprite = openedWindowSprite;
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


    public void OnClickPuzzle()
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

            // 퍼즐 완료 후 아이템을 획득 리스트에 추가
            string itemName = "WindowPuzzleItem"; // 기본 아이템 이름을 수동으로 지정
            if (GetComponent<IObjectItem>() != null)
            {
                itemName = GetComponent<IObjectItem>().ClickItem().itemName; // null 체크 후 아이템 이름 동적으로 가져오기
            }

            SceneDataManager.Instance.Data.acquiredItemIDs.Add(itemName);  // 동적으로 아이템 이름 추가

            if (windowRenderer != null && openedWindowSprite != null)
            {
                windowRenderer.sprite = openedWindowSprite;

                SoundManager.PlayOneShot(gameObject, openWindowClip, sfxMixerGroup);
            }

            if (clickableWindowObject != null)
            {
                clickableWindowObject.SetActive(true);
            }

            isWindowOpened = true;

            // 일정 지연 후 아이템 비활성화
            StartCoroutine(DisableItemAfterDelay());
        }
    }


    private IEnumerator DisableItemAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // 약간의 지연 후 비활성화
        PuzzleUtils.DisableAcquiredItemObjects();
    }
    private IEnumerator GoToStoryAfterDelay(float delay)
    {
        if (overlayImage != null)
        {
            overlayImage.SetActive(true);
            SpriteRenderer overlay = overlayImage.GetComponent<SpriteRenderer>();
            if (overlay != null)
            {
                Color color = overlay.color;
                color.a = 0f;
                overlay.color = color;

                float timer = 0f;
                float fadeDuration = 1f;

                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                    overlay.color = color;
                    yield return null;
                }
                color.a = 1f;
                overlay.color = color;
            }
        }

        yield return new WaitForSeconds(delay);

        SceneDataManager.Instance.Data.nextStoryIndex = nextStoryIndex;
        SceneManager.LoadScene("StoryScene");
    }
}