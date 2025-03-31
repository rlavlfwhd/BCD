using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    public CanvasGroup fadeCanvas; // ���̵� ȿ���� ���� CanvasGroup
    public float fadeDuration = 1.5f; // ���̵� �� ���� �ð�

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fadeCanvas.alpha = 1; // ó���� ������ ���� ȭ��
        while (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
        fadeCanvas.interactable = false;
        fadeCanvas.blocksRaycasts = false; // UI Ŭ�� ���� ����
    }
}
