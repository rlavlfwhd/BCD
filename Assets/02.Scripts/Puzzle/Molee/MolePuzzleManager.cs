using UnityEngine;

public class MolePuzzleManager : MonoBehaviour
{
    public static MolePuzzleManager Instance;

    public bool canChooseAnswer = false;
    private MoleController selectedMole;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
            Debug.Log("확인 가능한 상황으로 전환되었습니다.");
        }
    }

    public void ResetAnswerSelection()
    {
        canChooseAnswer = false;
        selectedMole = null;
    }
}
