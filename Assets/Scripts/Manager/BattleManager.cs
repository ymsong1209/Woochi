using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    public  BaseCharacter       currentCharacter;               //���� ���� ��������
    private BaseSkill           currentSelectedSkill;           //���� ���õ� ��ų
    private int                 currentRound;                   //���� �� ��������

    [SerializeField] private GameObject skillTriggerSelector;

    /// <summary>
    /// �Ʊ��̶� ������ �ο� ����
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    [SerializeField] private Formation allies;
    [SerializeField] private Formation enemies;

    #region �̺�Ʈ
    /// <summary>
    /// ĳ���� ���� ���۵� �� ȣ��Ǵ� �̺�Ʈ(UI ������Ʈ ��)
    /// </summary>
    public Action<BaseCharacter, bool> OnCharacterTurnStart;
    #endregion

    #region �ο� ����
    [Header("Boolean Variables")]
    private bool isSkillSelected = false;
    private bool isSkillExecuted = false;
    #endregion

    private void Start()
    {
        CurState = BattleState.IDLE;
        skillTriggerSelector = GetComponentInChildren<SkillTriggerSelector>()?.gameObject;
        skillTriggerSelector.SetActive(false);
    }

    /// <summary>
    /// DungeonInfoSO ������ �޾ƿͼ� �Ʊ��� ���� ��ġ�� ����
    /// </summary>
    public void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        skillTriggerSelector.SetActive(true);

        currentRound = 0;
        combatQueue.Clear();

        // �Ʊ�, ���� �����̼� �ʱ�ȭ
        allies.Initialize(GameManager.GetInstance.Allies);
        enemies.Initialize(dungeon.EnemyList);
        
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

        #region PreRound ���·� �Ѿ
        PreRound();
        #endregion

    }

    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// ĳ���͵��� ���� ����
    /// </summary>
    void PreRound()
    {
        CurState = BattleState.PreRound;
        ++currentRound;
        CheckBuffs(BuffTiming.RoundStart);
        //������ ���� ĳ���� ��� Ȯ��
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
    /// BuffTiming�� �Ű������� �޾Ƽ� �ش� ������ ������ ����
    /// </summary>
    void CheckBuffs(BuffTiming buffTiming)
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue���� �׸��� ����
            BaseCharacter character = combatQueue.Dequeue();

            character.ApplyBuff(buffTiming);

            // ������ character�� Queue�� ���ʿ� �ٽ� �߰�.
            combatQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// ĳ���͵��� �ӵ������� ����
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        //ĳ���͸� �ӵ������� �����ϸ鼭 ��� ������ ������ �� �ֵ��� ����
        ReorderCombatQueue(true, null);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueue�� �ٽ� �ӵ������� ����, ResetTurnUsed�� true�� �ϸ� ��� ĳ���Ͱ� ���� �ٽ� �� �� ����
    /// </summary>
    /// <param name="_resetTurnUsed">true�� ���� �� ��� ĳ���� �ٽ� �� ��밡��</param>
    /// <param name="processedCharacters"></param>
    void ReorderCombatQueue(bool _resetTurnUsed = false, List<BaseCharacter> processedCharacters = null)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>();

        if (processedCharacters != null)
        {
            allCharacters.AddRange(processedCharacters);
        }

        // combatQueue�� ���� �ִ� ĳ���͸� ��� allCharacters ����Ʈ�� �߰�
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters ����Ʈ�� �ӵ��� ���� ������
        allCharacters.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // �����ĵ� ����Ʈ�� �������� combatQueue �籸��
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
    /// ĳ���͵��� �ൿ ����
    /// </summary>
    void CharacterTurn()
    {
        CurState = BattleState.CharacterTurn;
        Debug.Log("CurState : CharacterTurn");
        //ĳ���ͺ��� �ൿ
        StartCoroutine(HandleCharacterTurns());
    }

    IEnumerator HandleCharacterTurns()
    {
        List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

        while (combatQueue.Count > 0)
        {
            #region ���� �Ͽ� ���� ���� �ʱ�ȭ
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

            // �ڽ��� ���ʰ� ���� �� ���� ����
            if (currentCharacter.ApplyBuff(BuffTiming.TurnStart))
            {
                // ĳ������ ��ų�� �������� �ִ��� Ȯ��
                // ���� ��ġ�� �ٲ� �� ������ ��ų Ȯ���� ����
                currentCharacter.CheckSkillsOnTurnStart();

                // ���� ���� ĳ���Ϳ� �´� UI ������Ʈ
                OnCharacterTurnStart?.Invoke(currentCharacter, true);

                // TODO : ���� ���� ���� �� AI�� �ൿ ����(�ӽ� �ڵ�)
                if (!currentCharacter.IsAlly)
                    StartCoroutine(EnemyAction(currentCharacter));
                
                // ��ų�� ���õǰ� ����� ������ ���
                while(!isSkillSelected || !isSkillExecuted)
                {
                    yield return null;
                }
            };

            // �ڽ� ���ʰ� ���� �� �� ��� ó��
            currentCharacter.IsTurnUsed = true;

            allies.ReOrder(); enemies.ReOrder();

            // ��ų ������� ���� �ӵ� ���� ó��
            ReorderCombatQueue(false, processedCharacters);

            processedCharacters.Add(currentCharacter);

            // �¸� ���� üũ
            if (CheckVictory(processedCharacters) && CheckVictory(combatQueue))
            {
                PostBattle(true);
                yield break;
            }
            //�й� ���� üũ
            else if (CheckDefeat(processedCharacters) && CheckDefeat(combatQueue))
            {
                PostBattle(false);
                yield break;
            }

            yield return null;
        }
        //ProcessedCharacter�� �ִ� ĳ���͵� �ٽ� characterQueue�� ����
        foreach(BaseCharacter characters in processedCharacters)
        {
            combatQueue.Enqueue(characters);
        }

        //��� ĳ������ ���� ������ �� ����
        PostRound();
    }

    /// <summary>
    /// UI���� ��ų ���� �� ȣ��Ǵ� �޼���
    /// </summary>
    public void SkillSelected(BaseSkill _selectedSkill)
    {
        // ����� ��ų ����
        currentSelectedSkill = _selectedSkill;
        isSkillSelected = true;
    }

    #region ��ų ���
    public void ExecuteSelectedSkill(int _index = -1)
    {
        if (!currentSelectedSkill) return;

        // ��ų ����� ĳ���� �ִϸ��̼� ����, ��ų ��� �� ��� ĳ���� �ִϸ��̼ǵ� �����ؾ� ��(ȸ�ǵ� �ִϸ��̼� �ֳ�) 
        BaseCharacter caster = currentSelectedSkill.SkillOwner;
        caster.PlayAnimation(currentSelectedSkill.SkillSO.AnimType);
        
        BaseCharacter receiver = null;
        
        //index<4�ΰ��� �Ʊ����� ��ų ����
        if (_index < 4)
        {
            receiver = allies.formation[_index];
        }
        //4<index<8�� ���� ������ ��ų ����
        else if (_index < 8)
        {
            receiver = enemies.formation[_index - 4];
        }

        if (currentSelectedSkill.SkillOwner && receiver)
        {
            StartCoroutine(ExecuteSkill(currentSelectedSkill.SkillOwner,receiver));
        }
    }

    // ��ų ���� ���� ����
    IEnumerator ExecuteSkill(BaseCharacter _caster, BaseCharacter receiver)
    {
        currentSelectedSkill.ActivateSkill(receiver);
        allies.CheckDeathInFormation();
        enemies.CheckDeathInFormation();
        
        Debug.Log(currentSelectedSkill.Name + " is executed by " + _caster.name + " on " + receiver.name);
        
        // caster�� �ִϸ��̼��� ��������� ��ٷȴٰ� ���� ����ǰ� ��
        while (!_caster.IsIdle) yield return null;

        isSkillExecuted = true;

        yield return new WaitForSeconds(1f); // ���÷� 1�� ���
    }
    
    /// <summary>
    /// Enemy �ӽ� �ൿ
    /// </summary>
    IEnumerator EnemyAction(BaseCharacter _enemy)
    {
        Debug.Log(_enemy.name + "�� �ൿ�մϴ�");
        yield return new WaitForSeconds(3f); // ���÷� 3�� ��� �� ��ų ���� ����
        isSkillSelected = true;
        isSkillExecuted = true;
    }

    #endregion

    public void TurnOver()
    {
        isSkillSelected = true;
        isSkillExecuted = true;
    }

    /// <summary>
    /// ���尡 ������ ����Ǵ� ���� ���� ��, �¸� ���� üũ
    /// </summary>
    void PostRound()
    {
        CurState = BattleState.PostRound;
        CheckBuffs(BuffTiming.RoundEnd);
        //������ ��� ������ PostBattle�� �Ѿ. �ƴҽ� �ٽ� PreRound�� ���ư�
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
    /// ������ ��� �׾����� Ȯ��
    /// </summary>
    bool CheckVictory(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (!character.IsAlly && !character.IsDead)
            {
                return false; // ����ִ� ������ �����Ƿ� �¸����� ����
            }
        }
        return true;
    }

    /// <summary>
    /// �Ʊ��� ��� �׾����� Ȯ��
    /// </summary>
    bool CheckDefeat(IEnumerable<BaseCharacter> characters)
    {
        foreach (BaseCharacter character in characters)
        {
            if (character.IsAlly && !character.IsDead)
            {
                return false; // ����ִ� �Ʊ��� �����Ƿ� �й����� ����
            }
        }
        return true;
    }

    /// <summary>
    /// ���� ���� ��, ���� ����
    /// </summary>
    void PostBattle(bool _victory)
    {
        //�¸���
        if (_victory)
        {
            //�¸� ȭ�� �� �� ���� ����
        }
        else
        {
            //�й� ȭ�� �߱�
        }
        //������ ��� ����
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
    /// �Ű������� ���� ĳ���Ͱ� ���� �����̼ǿ��� ��� ��ġ�� �ִ���
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
    /// index ��ġ�� ĳ���Ͱ� �ִ���
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
    /// ĳ������ ��ġ�� �̵���Ű�� �Լ�
    /// </summary>
    /// <param name="move">�󸶳� �̵��� ������, ������ �ڷ� �̵�, ����� ������ �̵�</param>
    public void MoveCharacter(BaseCharacter character, int move)
    {
        int from = GetCharacterIndex(character);
        int to = Mathf.Clamp(from - move, 0, 3);    // �̵��Ϸ��� ��ġ

        // �̵��� ���� ĳ���Ͱ� ������ �� ĳ������ RowOrder ���� ��ȯ
        // �ٲ� RowOrder ���� ���� ���� �� 
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
