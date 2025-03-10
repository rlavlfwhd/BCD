using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using System.Text;

#if UNITY_EDITOR
[CustomEditor(typeof(GameSystem))]
public class GameSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameSystem gameSystem = (GameSystem)target;

        // Reset Story Models 버튼 생성
        if (GUILayout.Button("Reset Story Models"))
        {
            gameSystem.ResetStoryModels();
        }
    }
}
#endif

public class GameSystem : MonoBehaviour
{
    public static GameSystem Instance;                 //간단한 싱글톤 화

    private void Awake()
    {
        Instance = this;
    }

    public enum GAMESTATE
    {
        STORYSHOW,
        WAITSELECT,
        STORYEND,
        BATTLEMODE,
        BATTLEDONE,
        SHOPMODE,
        ENDMODE
    }

    //=========================================================================================
    // 스텟
    //=========================================================================================


    public Stats stats;



    public GAMESTATE currentState;

    //=========================================================================================
    // 전투 
    //=========================================================================================

    // 공격 수식 상수
    private const int BASE_DAMAGE = 1; // 기본 피해량

    private System.Random random = new System.Random(); // 랜덤 숫자 생성기

    // 전투 수식 상수
    private const int D20_MAX = 20;
    private const int CRITICAL_HIT_THRESHOLD = 20;
    private const int CRITICAL_HIT_MULTIPLIER = 2;

    // 전투 로그를 저장할 StringBuilder 객체
    private StringBuilder battleLog = new StringBuilder();

    //=========================================================================================
    // 스토리
    //=========================================================================================

    public StoryModel[] storyModels;
    public int currentStoryIndex = 1;

#if UNITY_EDITOR
    [ContextMenu("Reset Story Models")]
    public void ResetStoryModels()
    {
        storyModels = Resources.LoadAll<StoryModel>(""); // Resources 폴더 아래 모든 StoryModel 불러오기
    }
#endif

    public void Start()
    {
        ChangeState(GAMESTATE.STORYSHOW);
    }

    public void ApplyChoice(StoryModel.Result result)
    {       
        switch (result.resultType)
        {
            case StoryModel.Result.ResultType.ChangeHp:
                stats.currentHpPoint += result.value;
                GameUI.Instance.UpdateHpUI();
                ChangeStats(result);
                break;

            case StoryModel.Result.ResultType.AddExperience:
                stats.currentXpPoint += result.value;
                GameUI.Instance.UpdateXpUI();
                ChangeStats(result);
                break;

            case StoryModel.Result.ResultType.GoToNextStory:
                currentStoryIndex = result.value;
                ChangeState(GAMESTATE.STORYSHOW);
                ChangeStats(result);
                break;

            case StoryModel.Result.ResultType.GoToRandomStory:
                RandomStory();
                ChangeState(GAMESTATE.STORYSHOW);
                ChangeStats(result);
                break;

            case StoryModel.Result.ResultType.GoToEnding:
                SceneManager.LoadScene(result.changeSceneName);
                break;

            default:
                Debug.LogError("Unknown effect type");
                break;
        }
    }

    public void ChangeStats(StoryModel.Result result)
    {
        if (result.stats.hpPoint > 0) stats.hpPoint += result.stats.hpPoint;
        if (result.stats.spPoint > 0) stats.spPoint += result.stats.spPoint;
        if (result.stats.currentHpPoint > 0) stats.currentHpPoint += result.stats.currentHpPoint;
        if (result.stats.currentSpPoint > 0) stats.currentSpPoint += result.stats.currentSpPoint;
        if (result.stats.currentXpPoint > 0) stats.currentXpPoint += result.stats.currentXpPoint;
        if (result.stats.strength > 0) stats.strength += result.stats.strength;
        if (result.stats.dexterity > 0) stats.dexterity += result.stats.dexterity;
        if (result.stats.consitiution > 0) stats.consitiution += result.stats.consitiution;
        if (result.stats.wisdom > 0) stats.wisdom += result.stats.wisdom;
        if (result.stats.Intelligence > 0) stats.Intelligence += result.stats.Intelligence;
        if (result.stats.charisma > 0) stats.charisma += result.stats.charisma;

    }

    public void ChangeState(GAMESTATE temp)
    {
        currentState = temp;

        if(currentState == GAMESTATE.STORYSHOW)
        {
            StoryShow(currentStoryIndex);
        }
    }

    public void StoryShow(int number)
    {

        StoryModel tempStoryModels = FindStoryModel(number);

        StorySystem.Instance.currentStoryModel = tempStoryModels;
        StorySystem.Instance.CoShowText();
    }

    StoryModel RandomStory()
    {
        StoryModel tempStoryModels = null;

        List<StoryModel> StoryModelList = new List<StoryModel>();

        for (int i = 0; i < storyModels.Length; i++)
        {
            if (storyModels[i].storytype == StoryModel.STORYTYPE.MAIN)
            {
                if(storyModels[i].storyLevelMax >= stats.currentXpPoint && stats.currentXpPoint >= storyModels[i].storyLevelMin)
                {
                    StoryModelList.Add(storyModels[i]);
                }                        
            }
        }

        tempStoryModels = StoryModelList[Random.Range(0, StoryModelList.Count)];

        currentStoryIndex = tempStoryModels.storyNumber;

        Debug.Log("currentStoryIndex" + currentStoryIndex);

        return tempStoryModels;
    }

    StoryModel FindStoryModel(int number)
    {
        StoryModel tempStoryModels = null;
        for (int i = 0; i < storyModels.Length; i++)
        {
            if (storyModels[i].storyNumber == number)
            {
                tempStoryModels = storyModels[i];
                break;
                
            }
        }

        return tempStoryModels;
    }

    //=========================================================================================
    // 전투 로그
    //=========================================================================================

    

    // 전투 시작 시 호출되는 함수
    public void StartBattle()
    {
        // 전투 로그 초기화
        battleLog.Clear();
        AppendToBattleLog("전투 시작!");

        // 여기서 전투 로직을 구현하고, 전투 상황에 따라서 AppendToBattleLog 함수를 호출하여 로그를 기록합니다.
        // 예를 들어,
        // AppendToBattleLog("적이 공격했습니다!");
        // AppendToBattleLog("플레이어가 반격했습니다!");

        // 전투 종료 후 전투 로그 출력
        Debug.Log(battleLog.ToString());
    }

    // 전투 로그에 내용 추가하는 함수
    private void AppendToBattleLog(string log)
    {
        battleLog.AppendLine(log);
    }

    //=========================================================================================
    // 전투
    //=========================================================================================

    public void Combat()
    {
        int attackRoll = RollD20(); // 공격 주사위 굴리기
        int damage = 0;

        if (attackRoll == D20_MAX) // 20이 나오면 크리티컬 히트
        {
            damage = RollDice(1, 6) * CRITICAL_HIT_MULTIPLIER;
            AppendToBattleLog("크리티컬 히트!");
        }
        else
        {
            // 피해량 계산 (일반 공격)
            damage = RollDice(1, 6);
        }

        // 피해량 적용 등 다른 전투 로직 처리
        // 예를 들어,
        // player.TakeDamage(damage);

        // 전투 로그에 결과 추가
        AppendToBattleLog($"공격 주사위 굴리기 결과: {attackRoll}");
        AppendToBattleLog($"피해량: {damage}");
    }

    // D20 주사위를 굴려 결과를 반환하는 함수
    private int RollD20()
    {
        return random.Next(1, D20_MAX + 1);
    }

    // 주사위를 numDice번 굴려 나온 값을 합산하여 반환하는 함수
    private int RollDice(int numDice, int diceSides)
    {
        int total = 0;
        for (int i = 0; i < numDice; i++)
        {
            total += random.Next(1, diceSides + 1);
        }
        return total;
    }

}
