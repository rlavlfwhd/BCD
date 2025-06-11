using UnityEngine;

/// <summary>
/// 초상화에 ID와 현재 슬롯 인덱스를 부여하는 스크립트
/// </summary>
public class PortraitWithID : MonoBehaviour
{
    public int ID; // 정답 비교용

    // 현재 이 Portrait가 어떤 슬롯 위치에 있는지를 나타냄
    public int CurrentIndex { get; private set; }

    // 슬롯 인덱스를 설정하는 함수
    public void SetIndex(int index)
    {
        CurrentIndex = index;
    }
}
