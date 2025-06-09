using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterPieceManager : MonoBehaviour
{
    public static LetterPieceManager Instance;

    // 얻은 편지 조각 번호 저장 (1~4)
    public List<int> foundPieces = new List<int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPiece(int index)
    {
        if (!foundPieces.Contains(index))
        {
            foundPieces.Add(index);
            foundPieces.Sort();
        }
    }

    public bool HasPiece(int index)
    {
        return foundPieces.Contains(index);
    }
}
