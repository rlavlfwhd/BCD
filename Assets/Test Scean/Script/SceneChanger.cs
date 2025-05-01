using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // 전환할 씬 이름
    [SerializeField] private string sceneToLoad = "GardenScene";

    // 마우스로 클릭했을 때
    private void OnMouseDown()
    {
        // 씬 전환
        SceneManager.LoadScene(sceneToLoad);
    }
}
