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
    public int nextStoryIndex = 200;

    [Header("사운드 매니저에 등록된 이름")]
    public string ropeUseSound; // 🧵 Rope를 사용할 때 사운드
    public string fadeInSound;  // 🌫️ 페이드 인할 때 사운드

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

            Debug.Log("3D 창문 열림! Rope4 사용 완료");

            // ✅ 창문 성공적으로 드랍했을 때 사운드 재생
            if (!string.IsNullOrEmpty(ropeUseSound))
            {
                SoundManager.instance.PlaySound(ropeUseSound);
            }
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

            // ✅ 페이드 인 시작할 때 사운드 재생
            if (!string.IsNullOrEmpty(fadeInSound))
            {
                SoundManager.instance.PlaySound(fadeInSound);
            }

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
