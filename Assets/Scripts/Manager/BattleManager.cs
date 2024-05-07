using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    public  BaseCharacter       currentCharacter;               //?„ì¬ ?„êµ¬ ì°¨ë??¸ì?
    private BaseSkill           currentSelectedSkill;           //?„ì¬ ? íƒ???¤í‚¬
    private int                 currentRound;                   //?„ì¬ ëª??¼ìš´?œì¸ì§€

    [SerializeField] private GameObject skillTriggerSelector;

    /// <summary>
    /// ?„êµ°?´ë‘ ?êµ°???¸ì? ?œì„œ
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    [SerializeField] private Formation allies;
    [SerializeField] private Formation enemies;

    [SerializeField] private AllyCardList allyCards;

    #region ?´ë²¤??
    /// <summary>
    /// ìºë¦­???´ì´ ?œì‘?????¸ì¶œ?˜ëŠ” ?´ë²¤??UI ?…ë°?´íŠ¸ ??
    /// </summary>
    public Action<BaseCharacter, bool> OnCharacterTurnStart;
    #endregion

    #region ë¶€??ë³€??
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
    /// DungeonInfoSO ?•ë³´ë¥?ë°›ì•„?€???„êµ°ê³??êµ° ?„ì¹˜ê°??¤ì •
    /// </summary>
    public void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        skillTriggerSelector.SetActive(true);

        currentRound = 0;
        combatQueue.Clear();

        // ?„êµ°, ?êµ° ?¬ë©”?´ì…˜ ì´ˆê¸°??
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

        #region PreRound ?íƒœë¡??˜ì–´ê°?
        PreRound();
        #endregion

    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// ìºë¦­?°ë“¤??ë²„í”„ ?•ë¦¬
    /// </summary>
    void PreRound()
    {
        CurState = BattleState.PreRound;
        ++currentRound;
        CheckBuffs(BuffTiming.RoundStart);
        //ë²„í”„ë¡??¸í•œ ìºë¦­???¬ë§ ?•ì¸
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
    /// BuffTiming??ë§¤ê°œë³€?˜ë¡œ ë°›ì•„???´ë‹¹ ?œì ??ë²„í”„ë¥??ìš©
    /// </summary>
    void CheckBuffs(BuffTiming buffTiming)
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue?ì„œ ??ª©???œê±°
            BaseCharacter character = combatQueue.Dequeue();

            character.ApplyBuff(buffTiming);

            // ?˜ì •??characterë¥?Queue???¤ìª½???¤ì‹œ ì¶”ê?.
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// ìºë¦­?°ë“¤???ë„?œìœ¼ë¡??•ë ¬
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        //ìºë¦­?°ë? ?ë„?œìœ¼ë¡??•ë ¬?˜ë©´??ëª¨ë‘ ?„íˆ¬??ì°¸ì—¬?????ˆë„ë¡?ë³€ê²?
        ReorderCombatQueue(true, null);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueueë¥??¤ì‹œ ?ë„?œìœ¼ë¡??•ë ¬, ResetTurnUsedë¥?trueë¡??˜ë©´ ëª¨ë“  ìºë¦­?°ê? ?´ì„ ?¤ì‹œ ?????ˆìŒ
    /// </summary>
    /// <param name="_resetTurnUsed">trueë¡??¤ì • ??ëª¨ë“  ìºë¦­???¤ì‹œ ???¬ìš©ê°€??/param>
    /// <param name="processedCharacters"></param>
    void ReorderCombatQueue(bool _resetTurnUsed = false, List<BaseCharacter> processedCharacters = null)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>();

        if (processedCharacters != null)
        {
            allCharacters.AddRange(processedCharacters);
        }

        // combatQueue???¨ì•„ ?ˆëŠ” ìºë¦­?°ë? ëª¨ë‘ allCharacters ë¦¬ìŠ¤?¸ì— ì¶”ê?
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters ë¦¬ìŠ¤?¸ë? ?ë„???°ë¼ ?¬ì •??
        allCharacters.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // ?¬ì •?¬ëœ ë¦¬ìŠ¤?¸ë? ë°”íƒ•?¼ë¡œ combatQueue ?¬êµ¬??
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
    /// ìºë¦­?°ë“¤???‰ë™ ?œì‘
    /// </summary>
    void CharacterTurn()
    {
        CurState = BattleState.CharacterTurn;
        Debug.Log("CurState : CharacterTurn");
        //ìºë¦­?°ë³„ë¡??‰ë™
        StartCoroutine(HandleCharacterTurns());
    }

    IEnumerator HandleCharacterTurns()
    {
        List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

        while (combatQueue.Count > 0)
        {
            #region ?´ì „ ?´ì— ?°ì¸ ë³€??ì´ˆê¸°??
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

            // ?ì‹ ??ì°¨ë?ê°€ ?ì„ ??ë²„í”„ ?ìš©
            if (currentCharacter.ApplyBuff(BuffTiming.TurnStart))
            {
                // ìºë¦­?°ì˜ ?¤í‚¬??ë³€ê²½ì ???ˆëŠ”ì§€ ?•ì¸
                // ?ë„ ?„ì¹˜ê°€ ë°”ë€????ˆìœ¼???¤í‚¬ ?•ì¸???´ì¤Œ
                currentCharacter.CheckSkillsOnTurnStart();

                // ?„ì¬ ?´ì˜ ìºë¦­?°ì— ë§ëŠ” UI ?…ë°?´íŠ¸
                OnCharacterTurnStart?.Invoke(currentCharacter, true);

                // TODO : ?„ì¬ ?´ì´ ?ì¼ ??AIë¡??‰ë™ ê²°ì •(?„ì‹œ ì½”ë“œ)
                if (!currentCharacter.IsAlly)
                {
                    //StartCoroutine(EnemyAction(currentCharacter));
                    EnemyAction(currentCharacter);
                }
                   
                
                // ?¤í‚¬??? íƒ?˜ê³  ?¤í–‰???Œê¹Œì§€ ?€ê¸?
                while(!isSkillSelected || !isSkillExecuted)
                {
                    yield return null;
                }
            };

            // ?ì‹  ì°¨ë?ê°€ ì§€???????¬ìš© ì²˜ë¦¬
            currentCharacter.IsTurnUsed = true;

            allies.ReOrder(); enemies.ReOrder();

            // ?¤í‚¬ ?¬ìš©?¼ë¡œ ?¸í•œ ?ë„ ë³€ê²?ì²˜ë¦¬
            ReorderCombatQueue(false, processedCharacters);

            processedCharacters.Add(currentCharacter);

            // ?¹ë¦¬ ì¡°ê±´ ì²´í¬
            if (CheckVictory(processedCharacters) && CheckVictory(combatQueue))
            {
                PostBattle(true);
                yield break;
            }
            //?¨ë°° ì¡°ê±´ ì²´í¬
            else if (CheckDefeat(processedCharacters) && CheckDefeat(combatQueue))
            {
                PostBattle(false);
                yield break;
            }

            yield return null;
        }
        //ProcessedCharacter???ˆëŠ” ìºë¦­?°ë“¤ ?¤ì‹œ characterQueue???½ì…
        foreach(BaseCharacter characters in processedCharacters)
        {
            combatQueue.Enqueue(characters);
        }

        //ëª¨ë“  ìºë¦­?°ì˜ ?´ì´ ?ë‚¬?????¤í–‰
        PostRound();
    }

    /// <summary>
    /// UI?ì„œ ?¤í‚¬ ? íƒ ???¸ì¶œ?˜ëŠ” ë©”ì„œ??
    /// </summary>
    public void SkillSelected(BaseSkill _selectedSkill)
    {
        // ?¬ìš©???¤í‚¬ ?€??
        currentSelectedSkill = _selectedSkill;
        isSkillSelected = true;
    }

    #region ?¤í‚¬ ?¬ìš©
    public void ExecuteSelectedSkill(int _index = -1)
    {
        if (!currentSelectedSkill) return;

        // ?¤í‚¬ ?¬ìš©??ìºë¦­??? ë‹ˆë©”ì´???¤í–‰, ?¤í‚¬ ?¬ìš© ???ë? ìºë¦­??? ë‹ˆë©”ì´?˜ë„ ?¤í–‰?´ì•¼ ???Œí”¼??? ë‹ˆë©”ì´???ˆë‚˜) 
        BaseCharacter caster = currentSelectedSkill.SkillOwner;
        caster.PlayAnimation(currentSelectedSkill.SkillSO.AnimType);
        
        BaseCharacter receiver = null;
        
        //index<4?¸ê²½?°ëŠ” ?„êµ°?ê²Œ ?¤í‚¬ ?ìš©
        if (_index < 4)
        {
            receiver = allies.formation[_index];
        }
        //4<index<8??ê²½ìš°???ì—ê²??¤í‚¬ ?ìš©
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

        // ?¤í‚¬ ?¬ìš©??ìºë¦­??? ë‹ˆë©”ì´???¤í–‰, ?¤í‚¬ ?¬ìš© ???ë? ìºë¦­??? ë‹ˆë©”ì´?˜ë„ ?¤í–‰?´ì•¼ ???Œí”¼??? ë‹ˆë©”ì´???ˆë‚˜) 
        BaseCharacter caster = currentSelectedSkill.SkillOwner;
        caster.PlayAnimation(currentSelectedSkill.SkillSO.AnimType);
        
        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
    }

    // ?¤í‚¬ ?¤í–‰ ë¡œì§ êµ¬í˜„
    IEnumerator ExecuteSkill(BaseCharacter _caster, BaseCharacter receiver)
    {
        currentSelectedSkill.ActivateSkill(receiver);
        allies.CheckDeathInFormation();
        enemies.CheckDeathInFormation();
        
        Debug.Log(currentSelectedSkill.Name + " is executed by " + _caster.name + " on " + receiver.name);
        
        // caster??? ë‹ˆë©”ì´?˜ì´ ?ë‚˜ê¸°ê¹Œì§€ ê¸°ë‹¤?¸ë‹¤ê°€ ?´ì´ ì¢…ë£Œ?˜ê²Œ ??
        while (!_caster.IsIdle) yield return null;

        isSkillExecuted = true;

        yield return new WaitForSeconds(1f); // ?ˆì‹œë¡?1ì´??€ê¸?
    }
    
    /// <summary>
    /// Enemy ?„ì‹œ ?‰ë™
    /// </summary>
    // IEnumerator EnemyAction(BaseCharacter _enemy)
    // {
    //     Debug.Log(_enemy.name + "ê°€ ?‰ë™?©ë‹ˆ??);
    //     yield return new WaitForSeconds(3f); // ?ˆì‹œë¡?3ì´??€ê¸????¤í‚¬ ?¤í–‰ ê°€??
    //     isSkillSelected = true;
    //     isSkillExecuted = true;
    // }

    void EnemyAction(BaseCharacter enemy)
    {
        enemy.TriggerAI();
        Debug.Log(enemy.name + "ê°€ ?‰ë™?©ë‹ˆ??");
    }

    #endregion

    public void TurnOver()
    {
        isSkillSelected = true;
        isSkillExecuted = true;
    }

    /// <summary>
    /// ?¼ìš´?œê? ?ë‚ ???ìš©?˜ëŠ” ë²„í”„ ?¤í–‰ ?? ?¹ë¦¬ ì¡°ê±´ ì²´í¬
    /// </summary>
    void PostRound()
    {
        CurState = BattleState.PostRound;
        CheckBuffs(BuffTiming.RoundEnd);
        //?êµ°??ëª¨ë‘ ì£½ìœ¼ë©?PostBattleë¡??˜ì–´ê°? ?„ë‹???¤ì‹œ PreRoundë¡??Œì•„ê°?
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
    /// ?êµ°??ëª¨ë‘ ì£½ì—ˆ?”ì? ?•ì¸
    /// </summary>
    bool CheckVictory(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (!character.IsAlly && !character.IsDead)
            {
                return false; // ?´ì•„?ˆëŠ” ?êµ°???ˆìœ¼ë¯€ë¡??¹ë¦¬?˜ì? ?ŠìŒ
            }
        }
        return true;
    }

    /// <summary>
    /// ?„êµ°??ëª¨ë‘ ì£½ì—ˆ?”ì? ?•ì¸
    /// </summary>
    bool CheckDefeat(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (character.IsAlly && !character.IsDead)
            {
                return false; // ?´ì•„?ˆëŠ” ?„êµ°???ˆìœ¼ë¯€ë¡??¨ë°°?˜ì? ?ŠìŒ
            }
        }
        return true;
    }

    /// <summary>
    /// ë³´ìƒ ?•ì‚° ?? ?„íˆ¬ ì¢…ë£Œ
    /// </summary>
    void PostBattle(bool _victory)
    {
        //?¹ë¦¬??
        if (_victory)
        {
            //?¹ë¦¬ ?”ë©´ ????ë³´ìƒ ?•ì‚°
        }
        else
        {
            //?¨ë°° ?”ë©´ ?¨ê¸°
        }
        //?êµ°??ê²½ìš° ?? œ
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
    /// ë§¤ê°œë³€?˜ë¡œ ?¤ì–´??ìºë¦­?°ê? ?„ì¬ ?¬ë©”?´ì…˜?ì„œ ?´ëŠ ?„ì¹˜???ˆëŠ”ì§€
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
    /// index ?„ì¹˜??ìºë¦­?°ê? ?ˆëŠ”ì§€
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
    /// ìºë¦­?°ì˜ ?„ì¹˜ë¥??´ë™?œí‚¤???¨ìˆ˜
    /// </summary>
    /// <param name="move">?¼ë§ˆ???´ë™??ê²ƒì¸ì§€, ?Œìˆ˜ë©??¤ë¡œ ?´ë™, ?‘ìˆ˜ë©??ìœ¼ë¡??´ë™</param>
    public void MoveCharacter(BaseCharacter character, int move)
    {
        int from = GetCharacterIndex(character);
        int to = Mathf.Clamp(from - move, 0, 3);    // ?´ë™?˜ë ¤???„ì¹˜

        // ?´ë™??ê³³ì— ìºë¦­?°ê? ?ˆìœ¼ë©???ìºë¦­?°ì˜ RowOrder ê°’ì„ êµí™˜
        // ë°”ë€?RowOrder ê°’ì? ?´ì´ ?ë‚  ??
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
