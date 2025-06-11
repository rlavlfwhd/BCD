using UnityEngine;

/// <summary>
/// �ʻ�ȭ�� ID�� ���� ���� �ε����� �ο��ϴ� ��ũ��Ʈ
/// </summary>
public class PortraitWithID : MonoBehaviour
{
    public int ID; // ���� �񱳿�

    // ���� �� Portrait�� � ���� ��ġ�� �ִ����� ��Ÿ��
    public int CurrentIndex { get; private set; }

    // ���� �ε����� �����ϴ� �Լ�
    public void SetIndex(int index)
    {
        CurrentIndex = index;
    }
}
