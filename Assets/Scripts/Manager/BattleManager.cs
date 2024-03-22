using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

public class BattleManager : SingletonMonobehaviour<GameManager>
{
   
    private BattleState         CurState;
    private BaseCharacter       currentCharacter;   //현재 누구 차례인지

    /// <summary>
    /// 아군이랑 적군을 CharacterList에 채움
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> CharacterQueue = new Queue<BaseCharacter>();

    private void Update()
    {
        if(CurState == BattleState.CharacterTurn)
        {

        }
    }


    /// <summary>
    /// DungeonInfoSO 정보를 받아와서 아군과 적군 위치값 설정
    /// </summary>
    void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        #region CharacterQueue 초기화
        CharacterQueue.Clear();
        #endregion

        #region 아군과 적군 배치
        // DungeonInfo내에 있는 적들을 알맞은 곳에 배치
        foreach (BaseCharacter enemy in dungeon.EnemyList)
        {
            CharacterQueue.Enqueue(enemy);
            //Todo : 캐릭터를 알맞은 곳에 배치
            Transform trans = enemy.transform;
            Instantiate(enemy,trans);
        }
       
        foreach(BaseCharacter ally in GameManager.GetInstance.Allies)
        {
            CharacterQueue.Enqueue(ally);
            //Todo : 아군 캐릭터 알맞게 배치
        }

        #endregion

        #region PreRound 상태로 넘어감
        PreRound();
        #endregion

    }

    /// <summary>
    /// 캐릭터들의 버프 정리
    /// </summary>
    void PreRound()
    {
        CurState = BattleState.PreRound;
        CheckPreBuffs();
        DetermineOrder();
    }

    /// <summary>
    /// 라운드가 시작할때 적용되는 버프 정리
    /// </summary>
    void CheckPreBuffs()
    {
        int characterCount = CharacterQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue에서 항목을 제거
            BaseCharacter character = CharacterQueue.Dequeue();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundStartBuff();
                //죽음 체크
                character.CheckDead();
            }

            // 수정된 character를 Queue의 뒤쪽에 다시 추가합니다.
            CharacterQueue.Enqueue(character);
        }

    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        SortBattleOrder();
        CharacterTurn();
    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void SortBattleOrder()
    {
        List<BaseCharacter> SortList = new List<BaseCharacter>();

        // Queue에 있는 요소 하나씩 꺼내서 List에 집어넣음
        foreach (BaseCharacter character in CharacterQueue)
        {
            SortList.Add(character);
        }

        // List 내부의 characters를 Speed 속성을 기준으로 내림차순 정렬
        SortList.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // Queue를 비우고
        CharacterQueue.Clear();

        // 정렬된 List 내부의 characters를 다시 CharacterQueue에 집어넣음
        foreach (BaseCharacter character in SortList)
        {
            CharacterQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 캐릭터들의 행동 시작
    /// </summary>
    void CharacterTurn()
    {
        CurState = BattleState.CharacterTurn;
        //캐릭터별로 행동
        StartCoroutine(HandleCharacterTurns());
    }

    IEnumerator HandleCharacterTurns()
    {
        List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

        while (CharacterQueue.Count > 0)
        {
            currentCharacter = CharacterQueue.Dequeue();

            if (currentCharacter.IsDead)
            {
                processedCharacters.Add(currentCharacter);
                continue;
            }

            // 차례가 됐을 때 버프 적용
            currentCharacter.ApplyTurnStartBuffs();



            yield return StartCoroutine(WaitForSkillSelection(currentCharacter));

            // 스킬 사용으로 인한 속도 변경 처리
            ReorderCharacterQueue(processedCharacters);

            processedCharacters.Add(currentCharacter);

            yield return new WaitForSeconds(1f); // 스킬 애니메이션 등을 위한 대기 시간
        }

        PostRound();
    }

    IEnumerator WaitForSkillSelection(BaseCharacter character)
    {
        // 스킬 선택 UI 활성화
        // 예: skillSelectionUI.ShowForCharacter(character);

        bool skillSelected = false;
        BaseSkill selectedSkill = null;

        // 스킬이 선택될 때까지 대기
        // 이 부분은 실제 UI 구현에 따라 달라질 수 있습니다.
        // 예를 들어, 스킬 선택 버튼이 클릭되면 아래의 람다 함수를 호출하도록 설정
        skillSelectionUI.OnSkillSelected += (skill) =>
        {
            selectedSkill = skill;
            skillSelected = true;
        };

        // 스킬이 선택될 때까지 무한 대기
        yield return new WaitUntil(() => skillSelected);

        // 선택된 스킬 실행
        if (selectedSkill != null)
        {
            yield return StartCoroutine(ExecuteSkill(selectedSkill, character));
        }

        // 스킬 선택 UI 비활성화
        // 예: skillSelectionUI.Hide();
    }

    IEnumerator ExecuteSkill(BaseSkill skill, BaseCharacter character)
    {
        // 스킬 실행 로직 구현
        // 예: 애니메이션 재생, 대미지 계산 등
        yield return new WaitForSeconds(1f); // 예시로 1초 대기
    }

    void ReorderCharacterQueue(List<BaseCharacter> processedCharacters)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>(processedCharacters);

        // CharacterQueue에 남아 있는 캐릭터를 모두 allCharacters 리스트에 추가
        while (CharacterQueue.Count > 0)
        {
            allCharacters.Add(CharacterQueue.Dequeue());
        }

        // allCharacters 리스트를 속도에 따라 재정렬
        allCharacters.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // 재정렬된 리스트를 바탕으로 CharacterQueue 재구성
        CharacterQueue.Clear();
        foreach (BaseCharacter character in allCharacters)
        {
            CharacterQueue.Enqueue(character);
        }
    }


    /// <summary>
    /// 라운드가 끝날때 적용되는 버프 실행 후, 승리 조건 체크
    /// </summary>
    void PostRound()
    {
        CurState = BattleState.PostRound;
        CheckPostBuffs();
        //적군이 모두 죽으면 PostBattle로 넘어감. 아닐시 다시 PreRound로 돌아감
        if(VictoryCheck())
        {
            PostBattle();
        }
        else
        {
            PreRound();
        }
    }

    void CheckPostBuffs()
    {
        int characterCount = CharacterQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue에서 항목을 제거
            BaseCharacter character = CharacterQueue.Dequeue();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundEndBuff();
                //죽음 체크
                character.CheckDead();
            }

            // 수정된 character를 Queue의 뒤쪽에 다시 추가합니다.
            CharacterQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 적군이 모두 죽었는지 확인
    /// </summary>
    bool VictoryCheck()
    {
        bool Victory = true;
        // Queue에 있는 요소 하나씩 꺼내서 List에 집어넣음
        foreach (BaseCharacter character in CharacterQueue)
        {
            if (Victory == false) continue;
            //적군인 경우
            if(character.IsAlly == false)
            {
                //적군이 살아있을 경우
                if(character.IsDead == false)
                {
                    Victory = false;
                    break;
                }
            }
        }
        return Victory;
    }

    /// <summary>
    /// 아군이 모두 죽었는지 확인
    /// </summary>
    bool DefeatCheck()
    {
        bool Defeat = true;
        // Queue에 있는 요소 하나씩 꺼내서 List에 집어넣음
        foreach (BaseCharacter character in CharacterQueue)
        {
            if (Defeat == false) continue;
            //아군인 경우
            if (character.IsAlly )
            {
                //아군이 살아있을 경우
                if (character.IsDead == false)
                {
                    Defeat = false;
                    break;
                }
            }
        }
        return Defeat;
    }

    /// <summary>
    /// 보상 정산 후, 전투 종료
    /// </summary>
    void PostBattle()
    {

    }
}
