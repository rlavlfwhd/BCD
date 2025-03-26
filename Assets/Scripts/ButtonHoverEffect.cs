using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ButtonHoverEffect : MonoBehaviour
{
    public Color hoverColor = Color.yellow; // 마우스 오버 시 색상
    private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

    void Start()
    {
        // 현재 오브젝트의 모든 자식 중 Button이 있는 경우 등록
        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage)
            {
                originalColors[button] = buttonImage.color; // 원래 색상 저장
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

                // 마우스 오버 이벤트 추가
                EventTrigger.Entry entryEnter = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerEnter
                };
                entryEnter.callback.AddListener((data) => OnHoverEnter(button));
                trigger.triggers.Add(entryEnter);

                // 마우스 나갈 때 이벤트 추가
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
