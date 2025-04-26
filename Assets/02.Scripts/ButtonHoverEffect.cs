using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonHoverEffect : MonoBehaviour
{
    public Color hoverColor = Color.yellow; // ���콺 ���� �� ����
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

    void Start()
    {
        // ���� ������Ʈ�� ��� �ڽ� �� Button�� �ִ� ��� ���
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage)
            {
                originalColors[button] = buttonImage.color; // ���� ���� ����
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

                // ���콺 ���� �̺�Ʈ �߰�
                EventTrigger.Entry entryEnter = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                entryEnter.callback.AddListener((data) => OnHoverEnter(button));
                trigger.triggers.Add(entryEnter);

                // ���콺 ���� �� �̺�Ʈ �߰�
                EventTrigger.Entry entryExit = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerExit
                };
                entryExit.callback.AddListener((data) => OnHoverExit(button));
                trigger.triggers.Add(entryExit);
            }
        }
    }

    private void OnHoverEnter(Button button)
    {
        if (button.TryGetComponent<Image>(out Image buttonImage))
        {
            buttonImage.color = hoverColor;
        }
    }

    private void OnHoverExit(Button button)
    {
        if (button.TryGetComponent<Image>(out Image buttonImage) && originalColors.ContainsKey(button))
        {
            buttonImage.color = originalColors[button];
        }
    }
}
