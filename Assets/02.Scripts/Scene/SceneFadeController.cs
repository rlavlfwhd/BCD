using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFadeController : MonoBehaviour
{
    // �� ��ȯ �� ����� �Լ�
    public void FadeAndLoadScene(string sceneName)
    {
        // �ڷ�ƾ�� ���� ������� ó��
        StartCoroutine(FadeOut_Load_FadeIn(sceneName));
    }

    private IEnumerator FadeOut_Load_FadeIn(string sceneName)
    {
        // 1. FadeOut: ȭ���� �˰� ���� ������ ���
        if (FadeManager.Instance != null)
        {
            // FadeOut �ڷ�ƾ�� �Ϸ�� ������ ���
            yield return StartCoroutine(FadeManager.Instance.FadeOut());
        }

        // 2. �� ��ȯ: �� ���� ���� �ε�
        //    Unity�� LoadScene�� �⺻������ ���� ȣ���̱� ������, ȣ�� ���� ���� ��ȯ�˴ϴ�.
        SceneManager.LoadScene(sceneName);

        // 3. �� ���� �ε�� ����, �������� �� �� ���ŵ� ������ ���
        //    (���� ������ �ε�� �� UI�� �ʱ�ȭ�� �ð��� Ȯ��)
        yield return null;

        // 4. FadeIn: ȭ���� ���������� ���� ������� ����
        if (FadeManager.Instance != null)
        {
            yield return StartCoroutine(FadeManager.Instance.FadeIn());
        }
    }
}
