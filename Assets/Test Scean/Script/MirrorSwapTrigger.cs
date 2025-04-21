using UnityEngine;

public class MirrorSwapTrigger : MonoBehaviour
{
    public GameObject mirrorOriginal;  // ���� Mirror ������Ʈ
    public GameObject mirrorAlternate; // �ٲ� Mirror2 ������Ʈ

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chicken")) // �� ������Ʈ�� "Chicken" �±� ����!
        {
            if (mirrorOriginal != null) mirrorOriginal.SetActive(false);
            if (mirrorAlternate != null) mirrorAlternate.SetActive(true);

            Debug.Log("���� �ſ￡ ���� �� Mirror ��ü��");
        }
    }
}
