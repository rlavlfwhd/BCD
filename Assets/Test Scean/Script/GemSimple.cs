using UnityEngine;

public class GemSimple : MonoBehaviour
{
    [Header("보석 이름 (\"Spring\", \"Summer\", \"Fall\", \"Winter\")")]
    public string gemName;

    private GemBoxPuzzleSimple puzzleManager;
    private SpriteRenderer sr;

    private void Awake()
    {
        puzzleManager = FindObjectOfType<GemBoxPuzzleSimple>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log($"[GemSimple] OnMouseDown fired on {this.gameObject.name}");
        if (puzzleManager != null)
        {
            puzzleManager.SelectGem(this);
        }
    }

    /// <summary>
    /// 슬롯으로 이동하기 전에 잠시 숨길 때 사용
    /// </summary>
    public void HideGem()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 슬롯 위치로 이동 후 다시 보이도록. 
    /// 슬롯의 X/Y를 사용하고, Z는 원래 그대로 유지합니다.
    /// </summary>
    public void ShowAtSlot(Transform slotTransform)
    {
        // 원래 보석의 Z 값을 저장
        float originalZ = this.transform.position.z;

        // 슬롯의 X/Y만 가져오고 Z는 기존 값 유지
        Vector3 newPos = new Vector3(
            slotTransform.position.x,
            slotTransform.position.y,
            originalZ
        );

        this.transform.position = newPos;
        this.gameObject.SetActive(true);
    }
}
