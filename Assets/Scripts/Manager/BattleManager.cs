using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    public  BaseCharacter       currentCharacter;               //현재 누구 차례인지
    private BaseSkill           currentSelectedSkill;           //현재 선택된 스킬
    private int                 currentRound;                   //현재 몇 라운드인지
    private int                 hardShip;                       // 역경 수치

    /// <summary>
    /// 아군이랑 적군의 싸움 순서
    /// </summary>
    private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    private List<BaseCharacter> processedCharacters = new List<BaseCharacter>();
    private HashSet<BaseCharacter> selectedCharacters = new HashSet<BaseCharacter>();   // 스킬 대상으로 선택된 캐릭터들

    [Header("Formation")]
    [SerializeField] private AllyFormation allies;
    [SerializeField] private Formation enemies;

    [Header("Object")]
    [SerializeField] private AllyCardList allyCards;
    [SerializeField] private Abnormal abnormal;     // 현재 노드의 이상(기본값 : None)
    [SerializeField] private BattleReward reward;   // 전투 보상

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

#if UNITY_EDITOR
    // 테스트해보고 싶은 적 캐릭터들이 있을 때 사용
    [Header("Test")]
    [SerializeField] private bool isTest = false;
    [SerializeField] private int[] testEnemy;
#endif

    private void Start()
    {
        CurState = BattleState.IDLE;

        InitializeAlly();
        #region 테스트할 수 있게
#if UNITY_EDITOR
        if (isTest)
        {
            DataCloud.dontSave = true;
            InitializeBattle(testEnemy);
        }
#endif
        #endregion
    }

    private void InitializeAlly()
    {
        var allyIDs = DataCloud.playerData.battleData.allies.ToArray();
        var allyList = GameManager.GetInstance.Library.GetCharacterList(allyIDs);

        allies.Initialize(allyList);
        allyCards.Initialize(allies);
    }

    /// <summary>
    /// DungeonInfoSO 정보를 받아와서 아군과 적군 위치값 설정
    /// </summary>
    public void InitializeBattle(int[] enemyIDs, int abnormalID = 100)
    {
        if (enemyIDs == null || enemyIDs.Length == 0) 
        { 
            Debug.LogError("Null Dungeon"); 
            return; 
        }

        CurState = BattleState.Initialization;

        currentRound = 0;
        combatQueue.Clear();
        processedCharacters.Clear();
        hardShip = 0;
        abnormal = GameManager.GetInstance.Library.GetAbnormal(abnormalID);

        var enemyList = GameManager.GetInstance.Library.GetCharacterList(enemyIDs);
        enemies.Initialize(enemyList);
        allyCards.UpdateList();

        foreach(Formation form in new Formation[] { allies, enemies })
        {
            for (int index = 0; index < form.formation.Length;)
            {
                if (form.formation[index] == null) break;
                combatQueue.Enqueue(form.formation[index]);
                index += form.formation[index].Size;
            }
        }

        InitializeAbnormal();
        UIManager.GetInstance.ActivateOpenMapUI(false);

        #region PreRound 상태로 넘어감
        PreRound();
        #endregion

    }

    private void InitializeAbnormal()
    {
        var buffList = abnormal.buffList;
        foreach (var buffPrefab in buffList)
        {
            AbnormalBuff buff = buffPrefab.GetComponent<AbnormalBuff>();

            if (buff.applyAlly)
            {
                var allyList = allies.GetCharacters();
                foreach (var ally in allyList)
                {
                    GameObject buffObject = Instantiate(buffPrefab, ally.transform);
                    AbnormalBuff abnormalBuff = buffObject.GetComponent<AbnormalBuff>();
                    ally.ApplyBuff(ally, ally, abnormalBuff);
                }
            }
            
            if(buff.applyEnemy)
            {
                var enemyList = enemies.GetCharacters();
                foreach (var enemy in enemyList)
                {
                    GameObject buffObject = Instantiate(buffPrefab, enemy.transform);
                    AbnormalBuff abnormalBuff = buffObject.GetComponent<AbnormalBuff>();
                    enemy.ApplyBuff(enemy, enemy, abnormalBuff);
                }
            }
        }
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
        ReorderCombatQueue(true);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueue를 다시 속도순으로 정렬, ResetTurnUsed를 true로 하면 모든 캐릭터가 턴을 다시 쓸 수 있음
    /// </summary>
    /// <param name="_resetTurnUsed">true로 설정 시 모든 캐릭터 다시 턴 사용가능</param>
    /// <param name="processedCharacters"></param>
    void ReorderCombatQueue(bool _resetTurnUsed = false)
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
        allCharacters.Sort((character1, character2) => character2.Stat.speed.CompareTo(character1.Stat.speed));

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

            // 자신의 차례가 됐을 때 버프 적용후, 살아있으면 턴 시작
            if (currentCharacter.TriggerBuff(BuffTiming.TurnStart))
            {
                // 현재 턴의 캐릭터에 맞는 UI 업데이트
                if(currentCharacter.IsAlly)
                    OnCharacterTurnStart?.Invoke(currentCharacter, true);
                
                //적군의 경우 AI 작동
                if (!currentCharacter.IsAlly)
                {
                    yield return new WaitForSeconds(1.5f);
                    EnemyAction(currentCharacter);
                }
                
                while (true)
                {
                    // 스킬이 선택되고 실행될 때까지 대기
                    while (!isSkillSelected || !isSkillExecuted)
                    {
                        yield return null;
                    }
                    allies.ReOrder(); enemies.ReOrder();

                    // 스킬 사용 후 턴 종료 조건을 만족하지 못하면 반복
                    if (currentSelectedSkill && !currentCharacter.CheckTurnEndFromSkillResult(currentSelectedSkill.SkillResult))
                    {
                        // 초기화 및 다시 대기
                        isSkillSelected = false;
                        isSkillExecuted = false;
                        currentSelectedSkill = null;

                        if (currentCharacter.IsAlly)
                        {
                            OnCharacterTurnStart?.Invoke(currentCharacter, true);
                        }
                        else
                        {
                            yield return new WaitForSeconds(1.5f);
                            EnemyAction(currentCharacter);
                        }
                    }
                    else
                    {
                        // 모든 조건이 만족되면 반복문 종료
                        break;
                    }
                }
            }

            // 자신 차례가 지난 후 턴 사용 처리
            currentCharacter.IsTurnUsed = true;
            // 턴이 종료된 후 버프 적용
            currentCharacter.TriggerBuff(BuffTiming.TurnEnd);
            
            allies.ReOrder(); enemies.ReOrder();

            // 스킬 사용으로 인한 속도 변경 처리
            ReorderCombatQueue(false);

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

        processedCharacters.Clear();

        //모든 캐릭터의 턴이 끝났을 때 실행
        PostRound();
    }

    /// <summary>
    /// 스킬 선택 시 호출되는 메서드
    /// 이제 선택 시 알아서 콜라이더, 화살표 활성화
    /// </summary>
    public void SkillSelected(BaseSkill _selectedSkill)
    {
        if(_selectedSkill == null) return;
        // 사용할 스킬 저장
        currentSelectedSkill = _selectedSkill;
        isSkillSelected = true;

        ActivateColliderArrow();
    }

    /// <summary>
    /// 스킬을 사용할 캐릭터를 선택하면 selectedCharacters에 추가
    /// 아군이 사용시 적 캐릭터를 클릭하면 자동으로 호출
    /// 적이 스킬을 사용할때는 수동으로 호출
    /// </summary>
    /// <param name="character"></param>
    public void CharacterSelected(BaseCharacter character)
    {
        if (selectedCharacters.Contains(character))
        {
            selectedCharacters.Remove(character);
        }
        else
        {
            selectedCharacters.Add(character);
        }
    }

    /// <summary>
    /// 스킬 사용할 캐릭터 선택 초기화
    /// </summary>
    public void InitSelect()
    {
        foreach(var character in selectedCharacters)
        {
            character.InitSelect();
        }
        selectedCharacters.Clear();
    }

    #region 콜라이더, 화살표 활성화
    public void ActivateColliderArrow()
    {
        ActivateColliderForSelectedSkill();
        EnableArrowForSelectedSkill();
    }

    private void ActivateColliderForSelectedSkill()
    {
        DisableAllColliderInteractions();
        bool[] skillRadius = currentSelectedSkill.SkillRadius;
        for (int i = 0; i < skillRadius.Length; i++)
        {
            //현재 살아있는 적/아군에게서만 collider활성화
            if(skillRadius[i] && IsCharacterThere(i))
            {
                BaseCharacter character = GetCharacterFromIndex(i);
                BaseCharacterCollider characterCollider = character.collider;
                characterCollider.CanInteract = true;
            }
        }
    }

    private void EnableArrowForSelectedSkill()
    {
        DisableAllArrows();
        bool[] skillRadius = currentSelectedSkill.SkillRadius;
        for (int i = 0; i < skillRadius.Length; i++)
        {
            //현재 살아있는 적/아군에게서만 화살표 활성화
            if(skillRadius[i] && IsCharacterThere(i))
            {
                BaseCharacter character = GetCharacterFromIndex(i);
                character.HUD.ActivateArrow(true);
            }
        }
    }

    public void DisableColliderArrow()
    {
        DisableAllColliderInteractions();
        DisableAllArrows();
    }

    private void DisableAllColliderInteractions()
    {
        foreach (Formation form in new Formation[] { allies, enemies })
        {
            foreach (BaseCharacter character in form.formation)
            {
                if (character)
                {
                    BaseCharacterCollider characterCollider = character.collider;
                    if (characterCollider)
                    {
                        characterCollider.CanInteract = false;
                    }
                }
            }
        }
    }

    private void DisableAllArrows()
    {
        foreach (Formation form in new Formation[] { allies, enemies })
        {
            foreach (BaseCharacter character in form.formation)
            {
                if (character)
                {
                    character.HUD.ActivateArrow(false);
                }
            }
        }
    }
    #endregion

    #region 스킬 사용
    public void ExecuteSelectedSkill(BaseCharacter receiver)
    {
        // 스킬 대상으로 지정한 캐릭터와 스킬 대상 수가 일치할 때 스킬 실행
        if (selectedCharacters.Count != currentSelectedSkill.SkillTargetCount) return;

        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
        
        //우치가 스킬을 사용한경우 도력감소
        if (currentCharacter.IsMainCharacter)
        {
            MainCharacter mainCharacter = currentCharacter as MainCharacter;
            MainCharacterSkill mainCharacterSkill = currentSelectedSkill as MainCharacterSkill;
            if (!mainCharacter) return;
            if (!mainCharacterSkill) return;
            mainCharacter.SorceryPoints -= mainCharacterSkill.RequiredSorceryPoints;
            Mathf.Clamp(mainCharacter.SorceryPoints, 0, mainCharacter.MaxSorceryPoints);
        }
    }

    // 스킬 실행 로직 구현
    IEnumerator ExecuteSkill(BaseCharacter caster, BaseCharacter receiver)
    {
        Debug.Log(currentSelectedSkill.Name + " is executed by " + caster.name + " on " + receiver.name);
        DisableColliderArrow();

        currentSelectedSkill.ActivateSkill(receiver);
        InitSelect();

        allies.CheckDeathInFormation();
        enemies.CheckDeathInFormation();

        OnFocusStart?.Invoke();

        // 더미 캐릭터가 receiver인 경우 caster의 UI를 활성화
        if (receiver.isDummy)
            OnCharacterAttacked?.Invoke(caster, false);
        else
            OnCharacterAttacked?.Invoke(receiver, false);
        
        yield return new WaitUntil(() => caster.IsIdle);
        OnFocusEnd?.Invoke();
        isSkillExecuted = true;
    }
    
    void EnemyAction(BaseCharacter enemycharacter)
    {
        BaseEnemy enemy = enemycharacter as BaseEnemy;
        enemy.TriggerAI();
        Debug.Log(enemy.name + "가 행동합니다");
    }

    #endregion

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
            //reward.ShowReward(10);
        }
        else
        {
            //패배 화면 뜨기
        }

        allies.BattleEnd(); enemies.BattleEnd();

        MapManager.GetInstance.CompleteNode();

        // 전투가 종료되었을때만 저장
        GameManager.GetInstance.SaveData();

        UIManager.GetInstance.ActivateOpenMapUI(true);
    }

    /// <summary>
    /// 매개변수로 들어온 캐릭터가 현재 포메이션에서 어느 위치에 있는지
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public int GetCharacterIndex(BaseCharacter character)
    {
        int index = -1;

        if(character.IsAlly)
        {
            index = allies.FindCharacterIndex(character);
        }
        else
        {
            index = enemies.FindCharacterIndex(character);
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

    #region 소환수 소환, 위치 이동 관련
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
                (allies.formation[to].RowOrder, character.RowOrder) = (character.RowOrder, allies.formation[to].RowOrder);
            }
        }
        else
        {
            if(IsCharacterThere(to + 4))
            {
                (enemies.formation[to].RowOrder, character.RowOrder) = (character.RowOrder, enemies.formation[to].RowOrder);
            }
        }
    }
    
    public void ChangeCharacterLocation()
    {
        List<BaseCharacter> list = new List<BaseCharacter>();
        foreach (var character in selectedCharacters)
        {
            list.Add(character);
        }

        (list[0].RowOrder, list[1].RowOrder) = (list[1].RowOrder, list[0].RowOrder);
    }

    /// <summary>
    /// 캐릭터를 소환
    /// </summary>
    /// <param name="_summon">소환할 캐릭터</param>
    /// <param name="_target">소환될 위치의 캐릭터</param>
    public void Summon(BaseCharacter _summon, BaseCharacter _target)
    {
        // 소환될 위치
        int index = GetCharacterIndex(_target);

        DisableDummy();

        if (allies.Summon(_summon, index))
        {
            processedCharacters.Add(_summon);
        }
    }

    public void UnSummon(BaseCharacter _character)
    {
        DisableDummy();

        // 턴 사용한 소환수 경우 -> 처리된 캐릭터 리스트에서 제거
        if(_character.IsTurnUsed)
        {
            foreach(var character in processedCharacters)
            {
                if (character == null) continue;
                if (character == _character)
                {
                    processedCharacters.Remove(character);
                    break;
                }
            }
        }
        // 턴 사용하지 않은 소환수 경우 -> combatQueue에서 제거
        else
        {
            Queue<BaseCharacter> tempQueue = new Queue<BaseCharacter>();

            while(combatQueue.Count > 0)
            {
                BaseCharacter character = combatQueue.Dequeue();
                if (character == _character)
                {
                    continue;
                }
                tempQueue.Enqueue(character);
            }

            combatQueue = tempQueue;
        }

        // 버프 제거
        _character.RemoveAllBuff();
        allies.UnSummon(_character);
        StartCoroutine(ExecuteSkill(currentCharacter, _character));
    }

    public void EnableDummy() => allies.EnableDummy();
    public void DisableDummy() => allies.DisableDummy();
    
    #endregion
    #region Getter Setter

    public Formation Allies => allies;

    public Formation Enemies => enemies;

    public BaseSkill CurrentSelectedSkill => currentSelectedSkill;
    #endregion
}
