using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    public  BaseCharacter       currentCharacter;               //?�재 ?�구 차�??��?
    private BaseSkill           currentSelectedSkill;           //?�재 ?�택???�킬
    private int                 currentRound;                   //?�재 �??�운?�인지

    [SerializeField] private GameObject skillTriggerSelector;

    /// <summary>
    /// ?�군?�랑 ?�군???��? ?�서
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    [SerializeField] private Formation allies;
    [SerializeField] private Formation enemies;

    [SerializeField] private AllyCardList allyCards;

    #region ?�벤??
    /// <summary>
    /// 캐릭???�이 ?�작?????�출?�는 ?�벤??UI ?�데?�트 ??
    /// </summary>
    public Action<BaseCharacter, bool> OnCharacterTurnStart;
    #endregion

    #region 부??변??
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
    /// DungeonInfoSO ?�보�?받아?�???�군�??�군 ?�치�??�정
    /// </summary>
    public void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        skillTriggerSelector.SetActive(true);

        currentRound = 0;
        combatQueue.Clear();

        // ?�군, ?�군 ?�메?�션 초기??
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

        // OnCharacterTurnStart?.Invoke(allyFormation[0], false);

        #region PreRound ?�태�??�어�?
        PreRound();
        #endregion

    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 캐릭?�들??버프 ?�리
    /// </summary>
    void PreRound()
    {
        CurState = BattleState.PreRound;
        ++currentRound;
        CheckBuffs(BuffTiming.RoundStart);
        //버프�??�한 캐릭???�망 ?�인
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
    /// BuffTiming??매개변?�로 받아???�당 ?�점??버프�??�용
    /// </summary>
    void CheckBuffs(BuffTiming buffTiming)
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue?�서 ??��???�거
            BaseCharacter character = combatQueue.Dequeue();

            character.ApplyBuff(buffTiming);

            // ?�정??character�?Queue???�쪽???�시 추�?.
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// 캐릭?�들???�도?�으�??�렬
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        //캐릭?��? ?�도?�으�??�렬?�면??모두 ?�투??참여?????�도�?변�?
        ReorderCombatQueue(true, null);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueue�??�시 ?�도?�으�??�렬, ResetTurnUsed�?true�??�면 모든 캐릭?��? ?�을 ?�시 ?????�음
    /// </summary>
    /// <param name="_resetTurnUsed">true�??�정 ??모든 캐릭???�시 ???�용가??/param>
    /// <param name="processedCharacters"></param>
    void ReorderCombatQueue(bool _resetTurnUsed = false, List<BaseCharacter> processedCharacters = null)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>();

        if (processedCharacters != null)
        {
            allCharacters.AddRange(processedCharacters);
        }

        // combatQueue???�아 ?�는 캐릭?��? 모두 allCharacters 리스?�에 추�?
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters 리스?��? ?�도???�라 ?�정??
        allCharacters.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // ?�정?�된 리스?��? 바탕?�로 combatQueue ?�구??
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
    /// 캐릭?�들???�동 ?�작
    /// </summary>
    void CharacterTurn()
    {
        CurState = BattleState.CharacterTurn;
        Debug.Log("CurState : CharacterTurn");
        //캐릭?�별�??�동
        StartCoroutine(HandleCharacterTurns());
    }

    IEnumerator HandleCharacterTurns()
    {
        List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

        while (combatQueue.Count > 0)
        {
            #region ?�전 ?�에 ?�인 변??초기??
            isSkillSelected = false;
            isSkillExecuted = false;
            currentSelectedSkill = null;
            #endregion
            currentCharacter = combatQueue.Dequeue();

            if (currentCharacter.IsDead || currentCharacter.IsTurnUsed)
            {
                processedCharacters.Add(currentCharacter);
                continue;
            }

            // ?�신??차�?가 ?�을 ??버프 ?�용
            if (currentCharacter.ApplyBuff(BuffTiming.TurnStart))
            {
                // 캐릭?�의 ?�킬??변경점???�는지 ?�인
                // ?�도 ?�치가 바�????�으???�킬 ?�인???�줌
                currentCharacter.CheckSkillsOnTurnStart();

                // ?�재 ?�의 캐릭?�에 맞는 UI ?�데?�트
                OnCharacterTurnStart?.Invoke(currentCharacter, true);

                // TODO : ?�재 ?�이 ?�일 ??AI�??�동 결정(?�시 코드)
                if (!currentCharacter.IsAlly)
                {
                    //StartCoroutine(EnemyAction(currentCharacter));
                    EnemyAction(currentCharacter);
                }
                   
                
                // ?�킬???�택?�고 ?�행???�까지 ?��?
                while(!isSkillSelected || !isSkillExecuted)
                {
                    yield return null;
                }
            };

            // ?�신 차�?가 지???????�용 처리
            currentCharacter.IsTurnUsed = true;

            allies.ReOrder(); enemies.ReOrder();

            // ?�킬 ?�용?�로 ?�한 ?�도 변�?처리
            ReorderCombatQueue(false, processedCharacters);

            processedCharacters.Add(currentCharacter);

            // ?�리 조건 체크
            if (CheckVictory(processedCharacters) && CheckVictory(combatQueue))
            {
                PostBattle(true);
                yield break;
            }
            //?�배 조건 체크
            else if (CheckDefeat(processedCharacters) && CheckDefeat(combatQueue))
            {
                PostBattle(false);
                yield break;
            }

            yield return null;
        }
        //ProcessedCharacter???�는 캐릭?�들 ?�시 characterQueue???�입
        foreach(BaseCharacter characters in processedCharacters)
        {
            combatQueue.Enqueue(characters);
        }

        //모든 캐릭?�의 ?�이 ?�났?????�행
        PostRound();
    }

    /// <summary>
    /// UI?�서 ?�킬 ?�택 ???�출?�는 메서??
    /// </summary>
    public void SkillSelected(BaseSkill _selectedSkill)
    {
        // ?�용???�킬 ?�??
        currentSelectedSkill = _selectedSkill;
        isSkillSelected = true;
    }

    #region ?�킬 ?�용
    public void ExecuteSelectedSkill(int _index = -1)
    {
        if (!currentSelectedSkill) return;

        // ?�킬 ?�용??캐릭???�니메이???�행, ?�킬 ?�용 ???��? 캐릭???�니메이?�도 ?�행?�야 ???�피???�니메이???�나) 
        BaseCharacter caster = currentSelectedSkill.SkillOwner;
        caster.PlayAnimation(currentSelectedSkill.SkillSO.AnimType);
        
        BaseCharacter receiver = null;
        
        //index<4?�경?�는 ?�군?�게 ?�킬 ?�용
        if (_index < 4)
        {
            receiver = allies.formation[_index];
        }
        //4<index<8??경우???�에�??�킬 ?�용
        else if (_index < 8)
        {
            receiver = enemies.formation[_index - 4];
        }

        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
    }
    
    public void ExecuteSelectedSkill(BaseCharacter receiver)
    {
        if (!currentSelectedSkill) return;

        // ?�킬 ?�용??캐릭???�니메이???�행, ?�킬 ?�용 ???��? 캐릭???�니메이?�도 ?�행?�야 ???�피???�니메이???�나) 
        BaseCharacter caster = currentSelectedSkill.SkillOwner;
        caster.PlayAnimation(currentSelectedSkill.SkillSO.AnimType);
        
        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
    }

    // ?�킬 ?�행 로직 구현
    IEnumerator ExecuteSkill(BaseCharacter _caster, BaseCharacter receiver)
    {
        currentSelectedSkill.ActivateSkill(receiver);
        allies.CheckDeathInFormation();
        enemies.CheckDeathInFormation();
        
        Debug.Log(currentSelectedSkill.Name + " is executed by " + _caster.name + " on " + receiver.name);
        
        // caster???�니메이?�이 ?�나기까지 기다?�다가 ?�이 종료?�게 ??
        while (!_caster.IsIdle) yield return null;

        isSkillExecuted = true;

        yield return new WaitForSeconds(1f); // ?�시�?1�??��?
    }
    
    /// <summary>
    /// Enemy ?�시 ?�동
    /// </summary>
    // IEnumerator EnemyAction(BaseCharacter _enemy)
    // {
    //     Debug.Log(_enemy.name + "가 ?�동?�니??);
    //     yield return new WaitForSeconds(3f); // ?�시�?3�??��????�킬 ?�행 가??
    //     isSkillSelected = true;
    //     isSkillExecuted = true;
    // }

    void EnemyAction(BaseCharacter enemy)
    {
        enemy.TriggerAI();
        Debug.Log(enemy.name + "가 ?�동?�니??");
    }

    #endregion

    public void TurnOver()
    {
        isSkillSelected = true;
        isSkillExecuted = true;
    }

    /// <summary>
    /// ?�운?��? ?�날???�용?�는 버프 ?�행 ?? ?�리 조건 체크
    /// </summary>
    void PostRound()
    {
        CurState = BattleState.PostRound;
        CheckBuffs(BuffTiming.RoundEnd);
        //?�군??모두 죽으�?PostBattle�??�어�? ?�닐???�시 PreRound�??�아�?
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
    /// ?�군??모두 죽었?��? ?�인
    /// </summary>
    bool CheckVictory(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (!character.IsAlly && !character.IsDead)
            {
                return false; // ?�아?�는 ?�군???�으므�??�리?��? ?�음
            }
        }
        return true;
    }

    /// <summary>
    /// ?�군??모두 죽었?��? ?�인
    /// </summary>
    bool CheckDefeat(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (character.IsAlly && !character.IsDead)
            {
                return false; // ?�아?�는 ?�군???�으므�??�배?��? ?�음
            }
        }
        return true;
    }

    /// <summary>
    /// 보상 ?�산 ?? ?�투 종료
    /// </summary>
    void PostBattle(bool _victory)
    {
        //?�리??
        if (_victory)
        {
            //?�리 ?�면 ????보상 ?�산
        }
        else
        {
            //?�배 ?�면 ?�기
        }
        //?�군??경우 ??��
        while (combatQueue.Count > 0)
        {
            BaseCharacter curchar = combatQueue.Dequeue();
            if(curchar.IsAlly == false)
            {
                curchar.Destroy();
                Destroy(curchar.gameObject);
            }
        }
        combatQueue.Clear();
        GameManager.GetInstance.SelectRoom();
    }

    /// <summary>
    /// 매개변?�로 ?�어??캐릭?��? ?�재 ?�메?�션?�서 ?�느 ?�치???�는지
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
    /// index ?�치??캐릭?��? ?�는지
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
    /// 캐릭?�의 ?�치�??�동?�키???�수
    /// </summary>
    /// <param name="move">?�마???�동??것인지, ?�수�??�로 ?�동, ?�수�??�으�??�동</param>
    public void MoveCharacter(BaseCharacter character, int move)
    {
        int from = GetCharacterIndex(character);
        int to = Mathf.Clamp(from - move, 0, 3);    // ?�동?�려???�치

        // ?�동??곳에 캐릭?��? ?�으�???캐릭?�의 RowOrder 값을 교환
        // 바�?RowOrder 값�? ?�이 ?�날 ??
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
