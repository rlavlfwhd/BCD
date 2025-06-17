using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SlotManager : MonoBehaviour
{
    public string puzzleID = "BookShelf";
    public BookSlot[] slots;
    public GameObject backgroundObject;
    public GameObject Door;
    public Item chickenStatueItem;
    public AudioClip bookshelfSlideClip;
    public AudioMixerGroup sfxMixerGroup;    

    private bool allSlotsCorrect = false;

    void OnEnable()
    {
        StartCoroutine(InitializePuzzleState());
    }

    private IEnumerator InitializePuzzleState()
    {
        yield return new WaitUntil(() => PuzzleManager.Instance != null);
        yield return null;

        if (PuzzleManager.Instance.IsPuzzleCompleted(puzzleID))
        {
            if (backgroundObject != null)
            {
                backgroundObject.transform.position += new Vector3(1100f, 0, 0);
                backgroundObject.SetActive(false);
            }

            if (Door != null)
            {
                Door.SetActive(true);
            }

            allSlotsCorrect = true;
        }
    }

    public void CheckSlotsNow()
    {
        if (!allSlotsCorrect && AreAllSlotsCorrect())
        {
            allSlotsCorrect = true;
            Debug.Log("🎉 모든 슬롯이 정답 책으로 채워졌습니다!");

            PuzzleManager.Instance.CompletePuzzle(puzzleID);
            StartCoroutine(TriggerPuzzleSuccessWithDelay());
        }
    }

    private IEnumerator TriggerPuzzleSuccessWithDelay()
    {
        yield return new WaitForSeconds(1f);

        Inventory.Instance.AddItem(chickenStatueItem);
        Door.SetActive(true);
        StartCoroutine(SlideOutBookshelf());
    }

    bool AreAllSlotsCorrect()
    {
        foreach (BookSlot slot in slots)
        {
            if (!slot.isOccupied || !slot.isCorrect)
                return false;
        }
        return true;
    }

    IEnumerator SlideOutBookshelf()
    {
        float distance = 1100f;
        float speed = 165f; // 초당 이동 속도
        float duration = distance / speed;

        float elapsed = 0f;

        Vector3 startPos = backgroundObject.transform.position;
        Vector3 endPos = startPos + new Vector3(distance, 0, 0);

        AudioSource sfx = backgroundObject.AddComponent<AudioSource>();
        sfx.clip = bookshelfSlideClip; // Inspector에서 연결
        if (sfxMixerGroup != null)
        {
            sfx.outputAudioMixerGroup = sfxMixerGroup;
        }
        sfx.Play();
        Destroy(sfx, bookshelfSlideClip.length); // 끝나면 제거

        while (elapsed < duration)
        {
            backgroundObject.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        backgroundObject.transform.position = endPos;
        backgroundObject.SetActive(false);
    }
}
