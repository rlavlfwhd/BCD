#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class AddButtonSFXHandlerEditor : MonoBehaviour
{
    [MenuItem("Tools/����/��� ��ư�� SFXHandler �߰�")]
    private static void AddSFXHandlerToAllButtons()
    {
        int count = 0;
        Button[] buttons = FindObjectsOfType<Button>(true); // ��Ȱ��ȭ ��ư���� ��� ã��

        foreach (Button button in buttons)
        {
            if (button.GetComponent<ButtonSFXHandler>() == null)
            {
                button.gameObject.AddComponent<ButtonSFXHandler>();
                count++;
            }
        }

        Debug.Log($"SFXHandler�� �߰��� ��ư ��: {count}��");
    }
}
#endif