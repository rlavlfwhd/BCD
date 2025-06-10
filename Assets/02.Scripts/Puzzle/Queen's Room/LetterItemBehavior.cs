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

    private bool isActive = false;
    private bool jewelsGiven = false; // 한 번만 주도록


    public void OnClickLetter()
    {
        var data = SceneDataManager.Instance.Data;
        isActive = !isActive;
        //letterPaperRoot.SetActive(isActive);

        if (darkFilterImage != null)
            darkFilterImage.SetActive(isActive);

        for (int i = 0; i < letterPaperObjects.Length; i++)
        {
            bool hasPiece = data.acquiredLetterPieces.Contains(i);
            letterPaperObjects[i].SetActive(isActive && hasPiece);
        }

        // **여기만 수정**
        if (isActive && data.acquiredLetterPieces.Count == 4 && !data.letterJewelsGiven)
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