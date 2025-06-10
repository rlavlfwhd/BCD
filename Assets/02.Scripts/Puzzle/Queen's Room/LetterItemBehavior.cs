using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterItemBehavior : MonoBehaviour
{    
    public GameObject letterPaperRoot;           // LetterContents(�г�, ó���� �����־�� ��)
    public GameObject[] letterPaperObjects;      // ������ ���� 4��
    public Item[] jewelItems;                    // ���� 4��
    public GameObject darkFilterImage;

    private bool isActive = false;
    private bool jewelsGiven = false; // �� ���� �ֵ���


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

        // **���⸸ ����**
        if (isActive && data.acquiredLetterPieces.Count == 4 && !data.letterJewelsGiven)
        {
            if (jewelItems != null && jewelItems.Length == 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    Inventory.Instance.AddItem(jewelItems[i]);
                }
                data.letterJewelsGiven = true;
                Debug.Log("���� 4�� �κ��丮�� ���� �Ϸ�");
            }
            else
            {
                Debug.LogError("jewelItems�� ����� �Ҵ���� �ʾҽ��ϴ�.");
            }
        }
    }
}