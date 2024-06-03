using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    public  BaseCharacter       currentCharacter;               //현재 누구 차례인지
    private BaseSkill           currentSelectedSkill;           //현재 선택된 스킬
    private int                 currentRound;                   //현재 몇 라운드인지
    

    /// <summary>
    /// 아군이랑 적군의 싸움 순서
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    [SerializeField] private Formation allies;
    [SerializeField] private Formation enemies;

    [SerializeField] private AllyCardList allyCards;

    #region 이벤트
    /// <summary>
    /// 캐릭터 턴이 시작될 때 호출되는 이벤트(UI 업데이트 등)
    /// </summary>
    public Action<BaseCharacter, bool> OnCharacterTurnStart;
    public Action<BaseCharacter, bool> OnCharacterAttacked;
    public Action OnFocusStart;
    public Action OnFocusEnd;
    public Action<BaseCharacter> OnFocusEnter;
    public Action<bool, bool> OnShakeCamera;
    #endregion

    #region 부울 변수
    [Header("Boolean Variables")]
    private bool isSkillSelected = false;
    private bool isSkillExecuted = false;
    #endregion

    private void Start()
    {
        CurState = BattleState.IDLE;

        allies.Initialize(GameManager.GetInstance.Allies);
        allyCards.Initialize(allies);
    }

    /// <summary>
    /// DungeonInfoSO 정보를 받아와서 아군과 적군 위치값 설정
    /// </summary>
    public void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        currentRound = 0;
        combatQueue.Clear();

        // 아군, 적군 포메이션 초기화
        enemies.Initialize(dungeon.EnemyList);
        allyCards.UpdateList();

        for (int index = 0; index < allies.formation.Length;)
        {
            if (allies.formation[index] == null) break;
            combatQueue.Enqueue(allies.formation[index]);
            index += allies.formation[index].Size;
        }

        for (int index = 0; index < enemies.formation.Length;)
        {
            if (enemies.formation[index] == null) break;
            combatQueue.Enqueue(enemies.formation[index]);
            index += enemies.formation[index].Size;
        }

        #region PreRound 상태로 넘어감
        PreRound();
        #endregion

    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 캐릭터들의 버프 정리
    /// </summary>
    void PreRound()
    {
        CurState = BattleState.PreRound;
        ++currentRound;
        CheckBuffs(BuffTiming.RoundStart);
        //버프로 인한 캐릭터 사망 확인
        if (CheckVictory(combatQueue))
        {
            PostBattle(true);
        }
        else if (CheckDefeat(combatQueue))
        {
            PostBattle(false);
        }
        DetermineOrder();
    }

    /// <summary>
    /// BuffTiming을 매개변수로 받아서 해당 시점에 버프를 적용
    /// </summary>
    void CheckBuffs(BuffTiming buffTiming)
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue에서 항목을 제거
            BaseCharacter character = combatQueue.Dequeue();

            character.TriggerBuff(buffTiming);

            // 수정된 character를 Queue의 뒤쪽에 다시 추가.
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        //캐릭터를 속도순으로 정렬하면서 모두 전투에 참여할 수 있도록 변경
        ReorderCombatQueue(true, null);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueue를 다시 속도순으로 정렬, ResetTurnUsed를 true로 하면 모든 캐릭터가 턴을 다시 쓸 수 있음
    /// </summary>
    /// <param name="_resetTurnUsed">true로 설정 시 모든 캐릭터 다시 턴 사용가능</param>
    /// <param name="processedCharacters"></param>
    void ReorderCombatQueue(bool _resetTurnUsed = false, List<BaseCharacter> processedCharacters = null)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>();
        
        if (_resetTurnUsed && processedCharacters != null)
        {
            allCharacters.AddRange(processedCharacters);
        }

        // combatQueue에 남아 있는 캐릭터를 모두 allCharacters 리스트에 추가
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters 리스트를 속도에 따라 재정렬
        allCharacters.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // 재정렬된 리스트를 바탕으로 combatQueue 재구성
        combatQueue.Clear();
        foreach (BaseCharacter character in allCharacters)
        {
            if (_resetTurnUsed)
            {
                character.IsTurnUsed = false;
            }
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 캐릭터들의 행동 시작
    /// </summary>
    void CharacterTurn()
    {
        CurState = BattleState.CharacterTurn;
        Debug.Log("CurState : CharacterTurn");
        //캐릭터별로 행동
        StartCoroutine(HandleCharacterTurns());
    }

    IEnumerator HandleCharacterTurns()
    {
        List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

        while (combatQueue.Count > 0)
        {
            #region 이전 턴에 쓰인 변수 초기화
            isSkillSelected = false;
            isSkillExecuted = false;
            currentSelectedSkill = null;
            #endregion
            currentCharacter = combatQueue.Dequeue();

            if (currentCharacter.IsDead || currentCharacter.IsTurnUsed)
            {
                Debug.Log(currentCharacter.name + " is dead or turn is used.");
                processedCharacters.Add(currentCharacter);
                continue;
            }

            // 자신의 차례가 됐을 때 버프 적용
            if (currentCharacter.TriggerBuff(BuffTiming.TurnStart))
            {
                // 현재 턴의 캐릭터에 맞는 UI 업데이트
                if(currentCharacter.IsAlly)
                    OnCharacterTurnStart?.Invoke(currentCharacter, true);

                // TODO : 현재 턴이 적일 시 AI로 행동 결정(임시 코드)
                if (!currentCharacter.IsAlly)
                {
                    yield return new WaitForSeconds(1.5f);
                    EnemyAction(currentCharacter);
                }
                
                // 스킬이 선택되고 실행될 때까지 대기
                while(!isSkillSelected || !isSkillExecuted)
                {
                    yield return null;
                }
            }

            // 자신 차례가 지난 후 턴 사용 처리
            currentCharacter.IsTurnUsed = true;
            // 턴이 종료된 후 버프 적용
            currentCharacter.TriggerBuff(BuffTiming.TurnEnd);
            
            allies.ReOrder(); enemies.ReOrder();

            // 스킬 사용으로 인한 속도 변경 처리
            ReorderCombatQueue(false, processedCharacters);

            processedCharacters.Add(currentCharacter);

            // 승리 조건 체크
            if (CheckVictory(processedCharacters) && CheckVictory(combatQueue))
            {
                PostBattle(true);
                yield break;
            }
            //패배 조건 체크
            else if (CheckDefeat(processedCharacters) && CheckDefeat(combatQueue))
            {
                PostBattle(false);
                yield break;
            }

            yield return null;
        }
        //ProcessedCharacter에 있는 캐릭터들 다시 characterQueue에 삽입
        foreach(BaseCharacter characters in processedCharacters)
        {
            combatQueue.Enqueue(characters);
        }

        //모든 캐릭터의 턴이 끝났을 때 실행
        PostRound();
    }

    /// <summary>
    /// UI에서 스킬 선택 시 호출되는 메서드
    /// </summary>
    public void SkillSelected(BaseSkill _selectedSkill)
    {
        // 사용할 스킬 저장
        currentSelectedSkill = _selectedSkill;
        isSkillSelected = true;
    }
    
    public void ActivateColliderForSelectedSkill()
    {
        if(currentSelectedSkill == null) return;
        DisableAllColliderInteractions();
        bool[] skillRadius = currentSelectedSkill.SkillRadius;
        for (int i = 0; i < skillRadius.Length; i++)
        {
            //현재 살아있는 적/아군에게서만 skilltriggerarea활성화
            if(skillRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
            {
                BaseCharacter character = BattleManager.GetInstance.GetCharacterFromIndex(i);
                BaseCharacterCollider characterCollider = character.GetComponent<BaseCharacterCollider>();
                characterCollider.CanInteract = true;
            }
        }
    }

    public void EnableArrowForSelectedSkill()
    {
        if(currentSelectedSkill == null) return;
        DisableAllArrows();
        bool[] skillRadius = currentSelectedSkill.SkillRadius;
        for (int i = 0; i < skillRadius.Length; i++)
        {
            //현재 살아있는 적/아군에게서만 skilltriggerarea활성화
            if(skillRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
            {
                BaseCharacter character = BattleManager.GetInstance.GetCharacterFromIndex(i);
                GameObject arrow = character.transform.Find("SkillSelectionArrow").gameObject;
                arrow.SetActive(true);
            }
        }
    }

    public void DisableAllColliderInteractions()
    {
        foreach (BaseCharacter character in allies.formation)
        {
            if (character)
            {
                BaseCharacterCollider characterCollider = character.GetComponent<BaseCharacterCollider>();
                if (characterCollider)
                {
                    characterCollider.CanInteract = false;
                }
            }
        }
        foreach (BaseCharacter character in enemies.formation)
        {
            if (character)
            {
                BaseCharacterCollider characterCollider = character.GetComponent<BaseCharacterCollider>();
                if (characterCollider)
                {
                    characterCollider.CanInteract = false;
                }
            }
        }
    }

    public void DisableAllArrows()
    {
        foreach (BaseCharacter character in allies.formation)
        {
            if (character)
            { 
                GameObject arrow = character.transform.Find("SkillSelectionArrow").gameObject;
                if (arrow)
                {
                    arrow.SetActive(false);
                }
            }
        }
        foreach (BaseCharacter character in enemies.formation)
        {
            if (character)
            {
                GameObject arrow = character.transform.Find("SkillSelectionArrow").gameObject;
                if (arrow)
                {
                    arrow.SetActive(false);
                }
            }
        }
    }
    
    public void ExecuteSelectedSkill(BaseCharacter receiver)
    {
        if (!currentSelectedSkill) return;
        
        DisableAllColliderInteractions();
        DisableAllArrows();
        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
    }

    #region 스킬 사용
    public void ExecuteSelectedSkill(int _index = -1)
    {
        if (!currentSelectedSkill) return;
        
        DisableAllColliderInteractions();
        DisableAllArrows();
        BaseCharacter receiver = null;
        
        //index<4인경우는 아군에게 스킬 적용
        if (_index < 4)
        {
            receiver = allies.formation[_index];
        }
        //4<index<8인 경우는 적에게 스킬 적용
        else if (_index < 8)
        {
            receiver = enemies.formation[_index - 4];
        }

        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
    }

    // 스킬 실행 로직 구현
    IEnumerator ExecuteSkill(BaseCharacter _caster, BaseCharacter receiver)
    {
        Debug.Log(currentSelectedSkill.Name + " is executed by " + _caster.name + " on " + receiver.name);

        currentSelectedSkill.ActivateSkill(receiver);
        allies.CheckDeathInFormation();
        enemies.CheckDeathInFormation();

        OnFocusStart?.Invoke();
        OnCharacterAttacked?.Invoke(receiver, false);
        
        yield return new WaitUntil(() => _caster.IsIdle);
        OnFocusEnd?.Invoke();
        isSkillExecuted = true;
    }
    
    void EnemyAction(BaseCharacter enemy)
    {
        enemy.TriggerAI();
        Debug.Log(enemy.name + "가 행동합니다");
    }

    #endregion

    public void TurnOver()
    {
        isSkillSelected = true;
        isSkillExecuted = true;
    }

    /// <summary>
    /// 라운드가 끝날때 적용되는 버프 실행 후, 승리 조건 체크
    /// </summary>
    void PostRound()
    {
        CurState = BattleState.PostRound;
        CheckBuffs(BuffTiming.RoundEnd);
        //적군이 모두 죽으면 PostBattle로 넘어감. 아닐시 다시 PreRound로 돌아감
        if(CheckVictory(combatQueue))
        {
            PostBattle(true);
        }
        else if (CheckDefeat(combatQueue))
        {
            PostBattle(false);
        }
        {
            PreRound();
        }
    }

    /// <summary>
    /// 적군이 모두 죽었는지 확인
    /// </summary>
    bool CheckVictory(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (!character.IsAlly && !character.IsDead)
            {
                return false; // 살아있는 적군이 있으므로 승리하지 않음
            }
        }
        return true;
    }

    /// <summary>
    /// 아군이 모두 죽었는지 확인
    /// </summary>
    bool CheckDefeat(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (character.IsAlly && !character.IsDead)
            {
                return false; // 살아있는 아군이 있으므로 패배하지 않음
            }
        }
        return true;
    }

    /// <summary>
    /// 보상 정산 후, 전투 종료
    /// </summary>
    void PostBattle(bool _victory)
    {
        //승리시
        if (_victory)
        {
            //승리 화면 뜬 후 보상 정산
        }
        else
        {
            //패배 화면 뜨기
        }
        
        //적군 삭제
        enemies.CleanUp();
        combatQueue.Clear();
        GameManager.GetInstance.SelectRoom();
    }

    /// <summary>
    /// 매개변수로 들어온 캐릭터가 현재 포메이션에서 어느 위치에 있는지
    /// </summary>
    /// <param name="_character"></param>
    /// <returns></returns>
    public int GetCharacterIndex(BaseCharacter _character)
    {
        int index = -1;

        if(_character.IsAlly)
        {
            index = allies.FindCharacter(_character);
        }
        else
        {
            index = enemies.FindCharacter(_character);
        }

        return index;
    }

    /// <summary>
    /// index 위치에 캐릭터가 있는지
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsCharacterThere(int index)
    {
        if(index < 4)
        {
            return allies.formation[index] != null;
        }
        else if(index < 8)
        {
            return enemies.formation[index - 4] != null;
        }

        return false;
    }
    
    /// <summary>
    /// index위치에 있는 캐릭터를 가져옴
    /// 캐릭터 없으면 null반환
    /// </summary>
    /// <param name="index"></param>
    /// <returns>0~4 : ally 5~8 : enemy</returns>
    public BaseCharacter GetCharacterFromIndex(int index)
    {
        if (index < 0 || index >= 8)
        {
            return null; // 인덱스가 유효 범위를 벗어남
        }

        // 유효한 인덱스 범위에 따라 적절한 포메이션과 실제 인덱스를 가져옴.
        (Formation formationClass, int localIndex) = GetFormationAndLocalIndex(index);

        bool isValidFormation = formationClass != null;
        bool isValidIndex = isValidFormation && localIndex >= 0 && localIndex < formationClass.formation.Length;

        // 유효한 경우에만 캐릭터를 반환, 아니면 null 반환
        return isValidIndex ? formationClass.formation[localIndex] : null;
    }
    
    private (Formation, int) GetFormationAndLocalIndex(int index)
    {
        if (index < 4)
        {
            return (allies, index);
        }
        else if (index < 8)
        {
            return (enemies, index - 4);
        }

        return (null, -1); // 유효하지 않은 인덱스
    }
    

    /// <summary>
    /// 캐릭터의 위치를 이동시키는 함수
    /// </summary>
    /// <param name="move">얼마나 이동할 것인지, 음수면 뒤로 이동, 양수면 앞으로 이동</param>
    public void MoveCharacter(BaseCharacter character, int move)
    {
        int from = GetCharacterIndex(character);
        int to = Mathf.Clamp(from - move, 0, 3);    // 이동하려는 위치

        // 이동한 곳에 캐릭터가 있으면 두 캐릭터의 RowOrder 값을 교환
        // 바뀐 RowOrder 값은 턴이 끝날 때 
        if(character.IsAlly)
        {
            if(IsCharacterThere(to))
            {
                (allies.formation[to].rowOrder, character.rowOrder) = (character.rowOrder, allies.formation[to].rowOrder);
            }
        }
        else
        {
            if(IsCharacterThere(to + 4))
            {
                (enemies.formation[to].rowOrder, character.rowOrder) = (character.rowOrder, enemies.formation[to].rowOrder);
            }
        }
    }
    
    public void MoveCharacter(BaseCharacter character, BaseCharacter target)
    {
        if(character == null || target == null) return;

        (target.rowOrder, character.rowOrder) = (character.rowOrder, target.rowOrder);
    }
    #region Getter Setter

    public Formation Allies => allies;

    public Formation Enemies => enemies;

    public BaseSkill CurrentSelectedSkill => currentSelectedSkill;
    #endregion
}
