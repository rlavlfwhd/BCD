using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterItemBehavior : MonoBehaviour
{    
    public GameObject letterPaperRoot;           // LetterContents(패널, 처음엔 꺼져있어야 함)
    public GameObject[] letterPaperObjects;      // 편지지 조각 4개
    public Item[] jewelItems;                    // 보석 4개
    public GameObject darkFilterImage;

    public void SetActiveLetterPanel(bool active)
    {
        var data = SceneDataManager.Instance.Data;

        if (darkFilterImage != null)
            darkFilterImage.SetActive(active);

        for (int i = 0; i < letterPaperObjects.Length; i++)
        {
            bool hasPiece = data.acquiredLetterPieces.Contains(i);
            letterPaperObjects[i].SetActive(active && hasPiece);
        }

        // 보석 지급 로직
        if (active && data.acquiredLetterPieces.Count == 4 && !data.letterJewelsGiven)
        {
            if (jewelItems != null && jewelItems.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    Inventory.Instance.AddItem(jewelItems[i]);
                }
                data.letterJewelsGiven = true;
                Debug.Log("보석 4개 인벤토리에 지급 완료");
            }
            else
            {
                Debug.LogError("jewelItems가 제대로 할당되지 않았습니다.");
            }
        }
    }
}