#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AddButtonSFXHandlerEditor : MonoBehaviour
{
    [MenuItem("Tools/사운드/모든 버튼에 SFXHandler 추가")]
    private static void AddSFXHandlerToAllButtons()
    {
        int count = 0;
        Button[] buttons = FindObjectsOfType<Button>(true); // 비활성화 버튼까지 모두 찾기

        foreach (Button button in buttons)
        {
            if (button.GetComponent<ButtonSFXHandler>() == null)
            {
                button.gameObject.AddComponent<ButtonSFXHandler>();
                count++;
            }
        }

        Debug.Log($"SFXHandler가 추가된 버튼 수: {count}개");
    }
}
#endif