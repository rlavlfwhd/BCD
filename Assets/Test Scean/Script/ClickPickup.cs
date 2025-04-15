using UnityEngine;
using TMPro; // TextMeshPro 사용을 위한 네임스페이스

public class ClickPickup : MonoBehaviour
{
    public GameObject pickupUIPanel; // 텍스트 창 전체 패널 오브젝트
    public TextMeshProUGUI pickupUIText; // 패널 안에 들어갈 텍스트 (TextMeshProUGUI)

    private Camera mainCamera; // 카메라 참조
    public float hideDelay = 2f; // 텍스트 창이 사라질 시간 (초)

    void Start()
    {
        mainCamera = Camera.main; // 메인 카메라 가져오기

        // 시작 시 텍스트 UI 창 비활성화
        if (pickupUIPanel != null)
            pickupUIPanel.SetActive(false);
    }

    void Update()
    {
        // 마우스 왼쪽 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            // 클릭한 위치를 월드 좌표로 변환
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // 해당 위치에 레이캐스트 발사
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // 무언가에 부딪혔다면
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                // 이 스크립트가 붙은 오브젝트를 클릭한 경우
                if (clickedObject == gameObject)
                {
                    Debug.Log("오브젝트 클릭됨: " + gameObject.name);

                    // 오브젝트 비활성화 (사라지게)
                    gameObject.SetActive(false);

                    // 텍스트 설정: 오브젝트 이름 + "획득"
                    pickupUIText.text = $"{gameObject.name} 획득";

                    // UI 창 보이게 하기
                    pickupUIPanel.SetActive(true);

                    // 일정 시간 후 UI 창 비활성화
                    Invoke("HidePickupUI", hideDelay);
                }
            }
        }
    }

    // 텍스트 UI 창을 숨기는 함수
    void HidePickupUI()
    {
        if (pickupUIPanel != null)
            pickupUIPanel.SetActive(false);
    }
}
