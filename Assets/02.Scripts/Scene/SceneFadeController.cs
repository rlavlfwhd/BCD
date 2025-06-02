using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFadeController : MonoBehaviour
{
    // 씬 전환 시 사용할 함수
    public void FadeAndLoadScene(string sceneName)
    {
        // 코루틴을 통해 순서대로 처리
        StartCoroutine(FadeOut_Load_FadeIn(sceneName));
    }

    private IEnumerator FadeOut_Load_FadeIn(string sceneName)
    {
        // 1. FadeOut: 화면이 검게 덮일 때까지 대기
        if (FadeManager.Instance != null)
        {
            // FadeOut 코루틴이 완료될 때까지 대기
            yield return StartCoroutine(FadeManager.Instance.FadeOut());
        }

        // 2. 씬 전환: 새 씬을 동기 로드
        //    Unity의 LoadScene은 기본적으로 동기 호출이기 때문에, 호출 직후 씬이 전환됩니다.
        SceneManager.LoadScene(sceneName);

        // 3. 새 씬이 로드된 직후, 프레임이 한 번 갱신될 때까지 대기
        //    (씬이 완전히 로드된 뒤 UI가 초기화될 시간을 확보)
        yield return null;

        // 4. FadeIn: 화면을 점진적으로 원래 모습으로 복원
        if (FadeManager.Instance != null)
        {
            yield return StartCoroutine(FadeManager.Instance.FadeIn());
        }
    }
}
