using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
    public  BaseCharacter       currentCharacter;               //현재 누구 차례인지
    private BaseSkill           currentSelectedSkill;           //현재 선택된 스킬
    private int                 currentRound;                   //현재 몇 라운드인지

    private HashSet<BaseCharacter> selectedCharacters = new HashSet<BaseCharacter>();   // 스킬 대상으로 선택된 캐릭터들

    [Header("Formation")]
    [SerializeField] private AllyFormation allies;
    [SerializeField] private Formation enemies;

    [Header("BattleResult")]
    [SerializeField] private BattleResultUI resultUI;   // 전투 결과 UI
    private BattleResult result;       // 전투 결과

    [Header("Object")]
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private AllyCardList allyCards;
    [SerializeField] private Abnormal abnormal;     // 현재 노드의 이상(기본값 : None)

    #region 이벤트
    /// <summary>
    /// 캐릭터 턴이 시작될 때 호출되는 이벤트(UI 업데이트 등)
    /// </summary>
    public Action<BaseCharacter, bool> ShowCharacterUI;

    public Action OnBattleStart;
    public Action OnFocusStart;
    public Action OnFocusEnd;
    public Action<BaseCharacter> OnFocusEnter;
    public Action<bool, bool> OnShakeCamera;
    public Action OnSkillExecuteFinished;
    public Action<Action> OnBattleEnd;
    #endregion

    #region 부울 변수
    [Header("Boolean Variables")]
    private bool isSkillSelected = false;
    private bool isSkillExecuted = false;
    #endregion
    
    private void Start()
    {
        InitializeAlly();
    }

    public void InitializeAlly()
    {
        var allyIDs = DataCloud.playerData.battleData.allies.ToArray();
        var allyList = GameManager.GetInstance.Library.GetCharacterList(allyIDs);

        //CreateAlly에서 다시 아군 정보 추가
        DataCloud.playerData.battleData.allies.Clear();
        
        allies.Initialize(allyList);
        allyCards.Initialize(allies);

        UIManager.GetInstance.sorceryGuageUI.Init();
    }

    public void AddAlly(GameObject prefab)
    {
        BaseCharacter ally = allies.CreateAlly(prefab);
        allyCards.Add(ally);
    }

    public void InitializeBattle(int[] enemyIDs, int abnormalID = 100, bool isElite = false)
    {
        if (enemyIDs == null || enemyIDs.Length == 0) 
        { 
            Debug.LogError("Null Dungeon"); 
            return; 
        }

        allies.AllCharacter.ForEach(c => c.InitializeSkill());

        currentRound = 0;
        abnormal = GameManager.GetInstance.Library.GetAbnormal(abnormalID);

        var enemyList = GameManager.GetInstance.Library.GetCharacterList(enemyIDs);
        enemies.Initialize(enemyList);

        // 턴 초기화
        turnManager.Init(allies, enemies);

        InitializeAbnormal();
        result.isElite = isElite;
        
        ShowCharacterUI?.Invoke(allies.GetWoochi(), false);
        GameManager.GetInstance.soundManager.PlaySFX("Fight_Start");
        GameManager.GetInstance.soundManager.PlayBGM(BGMState.Battle);
        OnBattleStart?.Invoke();

        GenerateBattleStartLog();
        
        #region PreRound 상태로 넘어감
        StopAllCoroutines();
        PreRound();
        #endregion
    }
    
    private void GenerateBattleStartLog()
    {
        if(MapManager.GetInstance.CurrentMap == null)
        {
            return;
        }
        var currentPoint = MapManager.GetInstance.CurrentMap.path[MapManager.GetInstance.CurrentMap.path.Count - 1];
        int currentFloor = currentPoint.y;
        
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"----Battle Start Log: Floor {currentFloor} ----");
        sb.AppendLine($"Difficulty (Hardship): {result.hardShipGrade + 1}");
        sb.AppendLine("-- Ally List --");

        for (int row = 0; row < allies.formation.Length; row++)
        {
            BaseCharacter character = allies.formation[row];
           
            if (character != null)
            {
                AppendCharacterInfo(sb, character, true);
                if(character.Size == 2)
                {
                    ++row;
                }
            }
        }

        sb.AppendLine("-- Enemy List --");

        for (int row = 0; row < enemies.formation.Length; row++)
        {
            BaseCharacter character = enemies.formation[row];
            if (character != null)
            {
                AppendCharacterInfo(sb, character, false);
                if(character.Size == 2)
                {
                    ++row;
                }
            }
        }

        sb.AppendLine("------------------------");
        
        Logger.Log(sb.ToString(), "BattleStart", "Floor" + currentFloor);
    }
    
    private void AppendCharacterInfo(System.Text.StringBuilder sb, BaseCharacter character, bool isAlly)
    {
        sb.AppendLine($"Row {character.RowOrder + 1} : {character.Name} (HP: {character.Health.CurHealth}/{character.Health.MaxHealth})");
        sb.AppendLine("  Stats:");

        foreach (var statValue in character.FinalStat.StatList)
        {
            sb.AppendLine($"    {statValue.type}: {statValue.value}");
        }

        sb.AppendLine($"  Level: {character.level.rank}, EXP: {character.level.exp}/{character.level.GetRequireExp()}");

        // 아군의 경우 스킬 정보를 추가
        if (isAlly && character is MainCharacter mainCharacter)
        {
            sb.AppendLine("  Skills:");
            foreach (BaseSkill skill in mainCharacter.MainCharacterSkills)
            {
                if (skill != null)
                {
                    sb.AppendLine($"    - {skill.Name}");
                }
            }
        }
    }

    private void InitializeAbnormal()
    {
        var buffList = abnormal.buffList;
        foreach (var buffPrefab in buffList)
        {
            AbnormalBuff buff = buffPrefab.GetComponent<AbnormalBuff>();

            if (buff.applyAlly)
            {
                var allyList = allies.AllCharacter;
                foreach (var ally in allyList)
                {
                    GameObject buffObject = Instantiate(buffPrefab, ally.transform);
                    AbnormalBuff abnormalBuff = buffObject.GetComponent<AbnormalBuff>();
                    ally.ApplyBuff(ally, ally, abnormalBuff);
                }
            }
            
            if(buff.applyEnemy)
            {
                var enemyList = enemies.AllCharacter;
                foreach (var enemy in enemyList)
                {
                    GameObject buffObject = Instantiate(buffPrefab, enemy.transform);
                    AbnormalBuff abnormalBuff = buffObject.GetComponent<AbnormalBuff>();
                    enemy.ApplyBuff(enemy, enemy, abnormalBuff);
                }
            }
        }
    }

    private void InitResult()
    {
        #region 역경 계산
        int hardShip = abnormal.cost;
        var enemyList = enemies.AllCharacter;
        foreach (var enemy in enemyList)
        {
            hardShip += enemy.Cost;
        }

        result.enemyCount = enemyList.Count;
        result.hardShipGrade = Mathf.Clamp(hardShip - 4 + DataCloud.playerData.CalculateLuck(), 0, 9);
        #endregion
    }


    /// 캐릭터들의 버프 정리
    /// </summary>
    void PreRound()
    {
        Logger.BattleLog($"---------------{currentRound}라운드 시작---------------", "RoundStart");
        ++currentRound;
        turnManager.SetRound(currentRound);
        turnManager.CheckBuffs(BuffTiming.RoundStart);
        //버프로 인한 캐릭터 사망 확인
        if (CheckVictory())
        {
            PostBattle();
        }
        DetermineOrder();
    }
    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>

    /// <summary>
    /// 캐릭터들을 속도순으로 정렬
    /// </summary>
    void DetermineOrder()
    {
        //캐릭터를 속도순으로 정렬하면서 모두 전투에 참여할 수 있도록 변경
        turnManager.ReorderCombatQueue(true);
        turnManager.PrintCombatQueue();
        CharacterTurn();
    }

    /// <summary>
    /// 캐릭터들의 행동 시작
    /// </summary>
    void CharacterTurn()
    {
        //캐릭터별로 행동
        StartCoroutine(HandleCharacterTurns());
    }

    IEnumerator HandleCharacterTurns()
    {
        WaitForSeconds delay = new WaitForSeconds(1.5f);
        
        while (turnManager.CombatQueue.Count > 0)
        {
            #region 이전 턴에 쓰인 변수 초기화
            isSkillSelected = false;
            isSkillExecuted = false;
            currentSelectedSkill = null;
            #endregion

            //우치 죽을때 시나리오 처리용으로 계속 대기
            while (turnManager.CanContinueTurn == false)
            {
                yield return null;
            }
            
            if (turnManager.StartTurn() == false)
            {
                continue;
            }
            
            Logger.BattleLog($"--------{currentCharacter.Name}({currentCharacter.RowOrder + 1}열)의 Turn--------", "TurnStart");

            // 자신의 차례가 됐을 때 버프 적용후, 살아있으면 턴 시작
            if (currentCharacter.TriggerBuff(BuffTiming.TurnStart))
            {
                ShowCharacterUI?.Invoke(currentCharacter, true);
                
                //적군의 경우 AI 작동
                if (!currentCharacter.IsAlly)
                {
                    yield return delay;
                    EnemyAction(currentCharacter);
                }
                
                while (currentCharacter.CheckUsableSkill())
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
                            ShowCharacterUI?.Invoke(currentCharacter, true);
                        }
                        else
                        {
                            yield return delay;
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
            else
            {
                allies.CheckDeathInFormation();
                enemies.CheckDeathInFormation();
                // 중간에 있는 애가 버프로 죽으면 바로 포메이션이 정렬돼서 여유를 둠
                yield return delay;
            }

            turnManager.EndTurn(currentCharacter);
            allies.ReOrder(); enemies.ReOrder();

            // 승리 조건 체크
            if (CheckVictory())
            {
                PostBattle();
                yield break;
            }
            
            ScenarioManager.GetInstance.NextPlot(PlotEvent.Action);
            
            yield return null;
        }

        turnManager.TurnOver();

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
            //SingularWithoutSelf는 skill범위에서 자기 자신을 선택 못하게 해야함.
            if(currentSelectedSkill.SkillTargetType == SkillTargetType.SingularWithoutSelf 
               && i == GetCharacterIndex(currentSelectedSkill.SkillOwner))
            {
                continue;
            }
            
            //Self는 skill범위에서 자기 자신만 선택 가능
            if(currentSelectedSkill.SkillTargetType == SkillTargetType.Self 
               && i != GetCharacterIndex(currentSelectedSkill.SkillOwner))
            {
                continue;
            }
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
            //SingularWithoutSelf는 skill범위에서 자기 자신을 선택 못하게 해야함.
            if(currentSelectedSkill.SkillTargetType == SkillTargetType.SingularWithoutSelf 
               && i == GetCharacterIndex(currentSelectedSkill.SkillOwner))
            {
                continue;
            }
            
            //Self는 skill범위에서 자기 자신만 선택 가능
            if(currentSelectedSkill.SkillTargetType == SkillTargetType.Self 
               && i != GetCharacterIndex(currentSelectedSkill.SkillOwner))
            {
                continue;
            }
            
            //현재 살아있는 적/아군에게서만 화살표 활성화
            if(skillRadius[i] && IsCharacterThere(i))
            {
                BaseCharacter character = GetCharacterFromIndex(i);
                character.anim.ActivateOutline();
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
                    character.anim.DeactivateOutline();
                }
            }
        }
    }
    #endregion

    #region 스킬 사용

    public void RemoveSelectedSkill()
    {
        currentSelectedSkill = null;
        isSkillSelected = false;
    }
    
    public void ExecuteSelectedSkill(BaseCharacter receiver)
    {
        // 스킬 대상으로 지정한 캐릭터와 스킬 대상 수가 일치할 때 스킬 실행
        if (selectedCharacters.Count != currentSelectedSkill.SkillTargetCount) return;

        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
    }

    // 스킬 실행 로직 구현
    IEnumerator ExecuteSkill(BaseCharacter caster, BaseCharacter receiver)
    {
        UIManager.GetInstance.DeactivePopup();

        DisableColliderArrow();
        if (!caster.IsAlly)
        {
            yield return UIManager.GetInstance.enemySkillNamePopup.ShowUI(currentSelectedSkill.Name);
        }
        
        currentSelectedSkill.PlaySound();
        currentSelectedSkill.ActivateSkill(receiver);
        InitSelect();

        allies.CheckDeathInFormation();
        enemies.CheckDeathInFormation();

        OnFocusStart?.Invoke();

        // 공격받은 캐릭터의 UI가 뜨는 코드
        // ChangeLoccation같은 스킬은 Deactivate때 Skill을 null로 초기화하기 때문에 아래 로직 임시로 주석
        // // 더미 캐릭터가 receiver인 경우 caster의 UI를 활성화 or 정비 중이면 caster의 UI를 활성화
        // if (receiver.isDummy)
        //     ShowCharacterUI?.Invoke(caster, false);
        // else
        //     ShowCharacterUI?.Invoke(receiver, false);
        
        yield return new WaitUntil(() => caster.IsIdle);
        
        OnFocusEnd?.Invoke();
        //ChangeLocation같은 스킬은 사용시 currentSelectedSkill을 null로 채움
        //하단 로직은 중독버프 같이 스킬로 트리거 되는 버프 체크용
        if (currentSelectedSkill)
        {
            
        }
        
        bool isAnyDead = false;
        foreach (BaseCharacter skillreceiver in currentSelectedSkill.SkillResult.Opponent)
        {
            //적이 살아있으면
            if (skillreceiver && !(skillreceiver.Health.CheckHealthZero() || skillreceiver.IsDead))
            {
                //버프 적용후 사망하면 대기
                if (!skillreceiver.TriggerBuff(BuffTiming.PostHit,currentSelectedSkill))
                {
                    isAnyDead = true;
                }
            }
        }

        if (isAnyDead)
        {
            yield return new WaitForSeconds(1f);
            allies.CheckDeathInFormation();
            enemies.CheckDeathInFormation();
        }
        
        OnSkillExecuteFinished?.Invoke();
        isSkillExecuted = true;
    }
    
    void EnemyAction(BaseCharacter enemycharacter)
    {
        BaseEnemy enemy = enemycharacter as BaseEnemy;
        enemy.TriggerAI();
    }

    #endregion

    /// <summary>
    /// 라운드가 끝날때 적용되는 버프 실행 후, 승리 조건 체크
    /// </summary>
    void PostRound()
    {
        turnManager.CheckBuffs(BuffTiming.RoundEnd);
        //적군이 모두 죽으면 PostBattle로 넘어감. 아닐시 다시 PreRound로 돌아감
        if(CheckVictory())
        {
            PostBattle();
        }
        else
        {
            PreRound();
        }
    }

    /// <summary>
    /// 적군이 모두 죽었는지 확인
    /// </summary>
    bool CheckVictory()
    {
        var enemyList = enemies.AllCharacter;
        for(int i = 0; i < enemyList.Count; i++)
        {
            // 적군이 한명이라도 살아있으면 false
            if (enemyList[i].IsDead == false)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 전투 정산 후, 전투 종료
    /// </summary>
    void PostBattle()
    {
        turnManager.BattleOver();
        InitResult();
        if (OnBattleEnd != null)
        {
            StartCoroutine(WaitForOnBattleEnd());
        }
        else
        {
            FinishBattle();
        }
    }
    private IEnumerator WaitForOnBattleEnd()
    {
        allies.BattleEnd(); //아군 정보 저장 먼저.
        
        bool isEnd = false;
        
        // OnBattleEnd에 대기 완료 콜백 전달
        OnBattleEnd.Invoke(() => isEnd = true);

        // OnBattleEnd 완료까지 대기
        while (!isEnd)
        {
            yield return null; // 매 프레임마다 대기
        }

        // 모든 대기가 끝난 후 후속 작업 진행
        FinishBattle();
    }

    private void FinishBattle()
    {
        allies.BattleEnd();
        enemies.BattleEnd();
        
        ScenarioManager.GetInstance.NextPlot(PlotEvent.BattleEnd);
        if (DataCloud.playerData.scenarioID == 0) return;
        
        
        //승리 화면 뜬 후 보상 정산
        resultUI?.Show(result);
        GenerateBattleEndLog();
    }
    
    private void GenerateBattleEndLog()
    {
        if(MapManager.GetInstance.CurrentMap == null)
        {
            return;
        }
        var currentPoint = MapManager.GetInstance.CurrentMap.path[MapManager.GetInstance.CurrentMap.path.Count - 1];
        int currentFloor = currentPoint.y;
        
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        sb.AppendLine($"----Battle End Log: Floor {currentFloor} ----");
        sb.AppendLine($"Difficulty (Hardship): {result.hardShipGrade + 1}");
        sb.AppendLine("-- Ally List --");

        for (int row = 0; row < allies.formation.Length; row++)
        {
            BaseCharacter character = allies.formation[row];
           
            if (character != null)
            {
                AppendCharacterInfo(sb, character, true);
                if(character.Size == 2)
                {
                    ++row;
                }
            }
        }

        sb.AppendLine("------------------------");
        
        Logger.BattleLog(sb.ToString(), "BattleEnd");
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
    /// index 위치에 살아있는 캐릭터가 있는지
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool IsCharacterThere(int index)
    {
        if(index < 4)
        {
            return (allies.formation[index] != null && allies.formation[index].IsDead == false);
        }
        else if(index < 8)
        {
            return (enemies.formation[index - 4] != null && enemies.formation[index-4].IsDead == false);
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
    /// <param name="isForce">강제로 이동시킬 것인지, 아니면 버프 체크 후 이동</param>
    public void MoveCharacter(BaseCharacter character, int move, bool isForce = true)
    {
        if (!isForce && character.activeBuffs.Count > 0)
        {
            foreach (var buff in character.activeBuffs)
            {
                if (buff.BuffEffect == BuffEffect.MoveResist)
                {
                    Logger.BattleLog($"\"{character.Name}\"은 이동 저항 버프로 이동할 수 없습니다.", "이동 불가");
                    return;
                }
            }
        }
        
        int from = GetCharacterIndex(character);
        int to = Mathf.Clamp(from - move, 0, 3);    // 이동하려는 위치
        if (character.IsAlly && GetCharacterFromIndex(to) && GetCharacterFromIndex(to) == character) //size가 2인 아군 캐릭터 고려
        {
            if (move > 0)
            {
                to = Mathf.Clamp(to - 1, 0, 3);
            }
            else
            {
                to = Mathf.Clamp(to + 1, 0, 3);
            }
        }
        else if (GetCharacterFromIndex(to + 4) && GetCharacterFromIndex(to + 4) == character) //size가 2인 적 캐릭터 고려
        {
            if (move > 0)
            {
                to = Mathf.Clamp(to - 1, 0, 3);
            }
            else
            {
                to = Mathf.Clamp(to + 1, 0, 3);
            }
        }

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
        Logger.BattleLog($"\"{list[0].Name}\"({list[0].RowOrder + 1}열)이(가) \"{list[1].Name}\"({list[1].RowOrder + 1}열)이랑 자리바꿈 시전", "우치 스킬[자리바꿈]");
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
            turnManager.Processed(_summon);
        }
    }

    public void UnSummon(BaseCharacter _character)
    {
        DisableDummy();

        turnManager.UnSummon(_character);

        // 버프 제거
        _character.RemoveAllBuff();
        allies.UnSummon(_character);
        StartCoroutine(ExecuteSkill(currentCharacter, _character));
    }

    public void EnableDummy() => allies.EnableDummy();
    public void DisableDummy() => allies.DisableDummy();

    #endregion

    #region 정비
    public void InitializeMaintenance()
    {
        MainCharacter woochi = allies.GetWoochi();
        currentCharacter = woochi;

        ShowCharacterUI?.Invoke(woochi, true);

        turnManager.OnlyWoochi(woochi);
        StartCoroutine(Maintenance());
    }

    IEnumerator Maintenance()
    {
        while(turnManager.CombatQueue.Count>0)
        {
            #region 변수 초기화
            isSkillSelected = false;
            isSkillExecuted = false;
            currentSelectedSkill = null;
            #endregion
            ShowCharacterUI?.Invoke(currentCharacter, true);

            while (!isSkillSelected || !isSkillExecuted)
            {
                yield return null;
            }

            allies.ReOrder();
            yield return null;
        }

        yield return null;
    }
    #endregion
    #region Getter Setter

    public AllyFormation Allies => allies;

    public Formation Enemies => enemies;

    public BaseSkill CurrentSelectedSkill => currentSelectedSkill;
    
    public TurnManager TurnManager => turnManager;
    #endregion
}
