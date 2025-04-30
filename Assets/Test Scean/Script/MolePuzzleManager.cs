using UnityEngine;

public class MolePuzzleManager : MonoBehaviour
{
    public static MolePuzzleManager Instance;

    [Header("정답 선택 가능 여부")]
    public bool canChooseAnswer = false;

    private MoleController selectedMole;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SelectMole(MoleController mole)
    {
        if (selectedMole != null && selectedMole != mole)
            selectedMole.ResetScale();

        selectedMole = mole;
        mole.Select();
    }

    public void AllowAnswerSelection()
    {
        if (!canChooseAnswer)
        {
            canChooseAnswer = true;
            Debug.Log("✅ 정답 선택 가능 상태로 전환됨 (가이드의 3번째 대사 출력)");
        }
    }

    public void ResetAnswerSelection()
    {
        canChooseAnswer = false;
        selectedMole = null;
    }
}

