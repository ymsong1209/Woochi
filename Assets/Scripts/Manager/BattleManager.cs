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

    #region �Ʊ��� ������ ��ġ��
    /// <summary>
    /// ũ�Ⱑ 1�� �Ʊ� ��ġ�� 
    /// 0~3 : 1��~4�� 
    /// </summary>
    [SerializeField] private Vector3[] allySinglePosition = new Vector3[4];
    /// <summary>
    /// ũ�Ⱑ 2�� �Ʊ� ��ġ��
    /// 0 : 1~2�� ����
    /// 1 : 2~3�� ����
    /// 2 : 3~4�� ����
    /// </summary>
    [SerializeField] private Vector3[] allyMultiplePosition = new Vector3[3];
    /// <summary>
    /// ũ�Ⱑ 1�� ���� ��ġ�� 
    /// 0~3 : 1��~4�� 
    /// </summary>
    [SerializeField] private Vector3[] enemySinglePosition = new Vector3[4];
    /// <summary>
    /// ũ�Ⱑ 2�� ���� ��ġ��
    /// 0 : 1~2�� ����
    /// 1 : 2~3�� ����
    /// 2 : 3~4�� ����
    /// </summary>
    [SerializeField] private Vector3[] enemyMultiplePosition = new Vector3[3];
    #endregion


    /// <summary>
    /// �Ʊ��̶� ������ �ο� ����
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    [SerializeField, ReadOnly] private BaseCharacter[] allyFormation = new BaseCharacter[4];
    [SerializeField, ReadOnly] private BaseCharacter[] enemyFormation = new BaseCharacter[4];

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
        for(int i = 0; i < allyFormation.Length; ++i)
        {
            allyFormation[i] = null;
        }
        for(int i = 0; i < enemyFormation.Length; ++i)
        {
            enemyFormation[i] = null;
        }

        #region �Ʊ��� ���� ��ġ
        #region ���� ��ġ
        // DungeonInfo���� �ִ� ������ �˸��� ���� ��ġ
        int EnemyTotalSize = 0;
        for(int i = 0; i < dungeon.EnemyList.Count; ++i)
        {
            GameObject enemyPrefab = dungeon.EnemyList[i];

            GameObject enemyGameObject = Instantiate(enemyPrefab);
            BaseCharacter enemyCharacter = enemyGameObject.GetComponent<BaseCharacter>();

            enemyCharacter.Initialize();
            enemyCharacter.IsAlly = false;

            //�� �Һ� üũ
            enemyCharacter.IsTurnUsed = false;
            //���� ���۽� ����Ǵ� ���� ����
            enemyCharacter.ApplyBuff(BuffTiming.BattleStart);

            //���� ������ ����
            combatQueue.Enqueue(enemyCharacter);
            enemyFormation[EnemyTotalSize] = enemyCharacter;

            //��ġ���� ���ؾ��ϴ� Ư���� ��ü�� �ƴϸ� enemyPosition��� ���� ��ȯ
            int enemySize = enemyCharacter.Size;
            if (enemyCharacter.IsSpawnSpecific == false)
            {
                //ũ�Ⱑ 1�� ����
                if (enemySize == 1)
                {
                    enemyGameObject.transform.position = enemySinglePosition[EnemyTotalSize];
                    ++EnemyTotalSize;
                }
                //ũ�Ⱑ 2�� ����
                else if (enemySize == 2) {
                    enemyGameObject.transform.position = enemyMultiplePosition[EnemyTotalSize];
                    enemyFormation[EnemyTotalSize + 1] = enemyCharacter;
                    EnemyTotalSize += 2;
                }
            }
            //��ġ�� ��������ϴ� Ư���� ��ü
            else
            {
                //ũ�Ⱑ 1�� ����
                if (enemySize == 1)
                {
                    enemyGameObject.transform.position = enemyCharacter.SpawnLocation;
                    ++EnemyTotalSize;
                }
                //ũ�Ⱑ 2�� ����
                else if (enemySize == 2)
                {
                    enemyGameObject.transform.position = enemyCharacter.SpawnLocation;
                    enemyFormation[EnemyTotalSize + 1] = enemyCharacter;
                    EnemyTotalSize += 2;
                }
            }
        }
        if (EnemyTotalSize > 4) Debug.LogError("EnemyTotalSize over 4");
        #endregion ���� ��ġ
        #region �Ʊ� ��ġ
        int AllyTotalSize = 0;
        for (int i = 0; i < GameManager.GetInstance.Allies.Count; ++i)
        {
            GameObject allyPrefab = GameManager.GetInstance.Allies[i];

            GameObject allyGameObject = Instantiate(allyPrefab);
            BaseCharacter allyCharacter = allyGameObject.GetComponent<BaseCharacter>();

            allyCharacter.Initialize();
            allyCharacter.IsAlly = true;

            //���� ������ ����
            combatQueue.Enqueue(allyCharacter);
            allyFormation[AllyTotalSize] = allyCharacter;

            //�� �Һ� üũ
            allyCharacter.IsTurnUsed = false;
            //���� ���� �� ����Ǵ� ���� ����
            allyCharacter.ApplyBuff(BuffTiming.BattleStart);

            //��ġ���� ���ؾ��ϴ� Ư���� ��ü�� �ƴϸ� allyPosition��� ���� ��ȯ
            int allySize = allyCharacter.Size;
            if (allyCharacter.IsSpawnSpecific == false)
            {
                //ũ�Ⱑ 1�� �Ʊ�
                if (allySize == 1)
                {
                    allyGameObject.transform.position = allySinglePosition[AllyTotalSize];
                    ++AllyTotalSize;
                }
                //ũ�Ⱑ 2�� �Ʊ�
                else if (allySize == 2)
                {
                    allyGameObject.transform.position = allyMultiplePosition[AllyTotalSize];
                    allyFormation[AllyTotalSize + 1] = allyCharacter;
                    AllyTotalSize += 2;
                }
            }
            //��ġ�� ��������ϴ� Ư���� ��ü
            else
            {
                //ũ�Ⱑ 1�� �Ʊ�
                if (allySize == 1)
                {
                    allyGameObject.transform.position = allyCharacter.SpawnLocation;
                    ++AllyTotalSize;
                }
                //ũ�Ⱑ 2�� �Ʊ�
                else if (allySize == 2)
                {
                    allyGameObject.transform.position = allyCharacter.SpawnLocation;
                    allyFormation[AllyTotalSize + 1] = allyCharacter;
                    AllyTotalSize += 2;
                }
            }
        }

        #endregion �Ʊ� ��ġ
        #endregion �Ʊ��� ���� ��ġ

        OnCharacterTurnStart?.Invoke(allyFormation[0], false);
        #region PreRound ���·� �Ѿ
        PreRound();
        #endregion

    }

    /// <summary>
    /// ĳ���͵��� ���� ����
    /// </summary>
    void PreRound()
    {
        CurState = BattleState.PreRound;
        ++currentRound;
        CheckPreBuffs();
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
    /// ���尡 �����Ҷ� ����Ǵ� ���� ����
    /// </summary>
    void CheckPreBuffs()
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue���� �׸��� ����
            BaseCharacter character = combatQueue.Dequeue();

            character.ApplyBuff(BuffTiming.RoundStart);

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
        ReordercombatQueue(true);
        CharacterTurn();
    }

    /// <summary>
    /// combatQueue�� �ٽ� �ӵ������� ����, ResetTurnUsed�� true�� �ϸ� ��� ĳ���Ͱ� ���� �ٽ� �� �� ����
    /// </summary>
    void ReordercombatQueue(bool _ResetTurnUsed = false)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>();

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
            if (_ResetTurnUsed)
            {
                character.IsTurnUsed = false;
            }
            combatQueue.Enqueue(character);
        }
    }

    void ReordercombatQueue(List<BaseCharacter> processedCharacters)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>(processedCharacters);

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

            // ��ų ������� ���� �ӵ� ���� ó��
            ReordercombatQueue(processedCharacters);

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

    public void ExecuteSelectedSkill(int _index = -1)
    {
        if (currentSelectedSkill == null) return;

        // ��ų ����� ĳ���� �ִϸ��̼� ����, ��ų ��� �� ��� ĳ���� �ִϸ��̼ǵ� �����ؾ� ��(ȸ�ǵ� �ִϸ��̼� �ֳ�) 
        BaseCharacter caster = currentSelectedSkill.SkillOwner;
        caster.PlayAnimation(currentSelectedSkill.SkillSO.AnimType);

        //���ϰ��� ��ų�� ��� index�� ���� ���� ����
        if(currentSelectedSkill.SkillTargetType == SkillTargetType.Singular)
        {
            AttackSingular(_index);
        }
        else if(currentSelectedSkill.SkillTargetType == SkillTargetType.Multiple)
        {
            AttackMultiple(_index);
        }
    }

    /// <summary>
    /// Index�� ��ġ�� ���� �� �Ѹ��� �����Ѵ�.
    /// </summary>
    private void AttackSingular(int _index)
    {
        BaseCharacter Caster = null;
        List<BaseCharacter> Enemies = new List<BaseCharacter>();

        if (CurrentSelectedSkill == null) return;
        Caster = currentSelectedSkill.SkillOwner;


        //index<4�ΰ��� ���� �Ʊ��� ������ ���
        if (_index < 4)
        {
           Enemies.Add(allyFormation[_index]);
        }
        //4<index<8�� ���� �Ʊ��� ���� ������ ���
        else if (_index < 8)
        {
            Enemies.Add(enemyFormation[_index - 4]);
        }
       
        if (Caster != null && Enemies.Count > 0)
        {
            StartCoroutine(ExecuteSkill(Caster, Enemies));
        }
        else
        {
            Debug.LogError("Caster or Enemy not assigned!");
        }
    }

    private void AttackMultiple(int _index)
    {
        BaseCharacter Caster = null;
        List<BaseCharacter> Receivers = new List<BaseCharacter>();
        if (currentSelectedSkill == null) return;
        Caster = currentSelectedSkill.SkillOwner;

        //Index ���ο� ������� selectedSkill�� ���� ���� ��� ���� ������ ���� ����
        for (int i = 0; i < currentSelectedSkill.SkillRadius.Length; i++)
        {
            //i<4�� ��� ���� �Ʊ��� ����
            if (i < 4 && currentSelectedSkill.SkillRadius[i])
            {
                BaseCharacter ally = allyFormation[i];
                if(ally == null) continue;

                //�Ʊ��� Size�� 2�� ���
                if (ally.Size == 2)
                {
                    // �̹� Receivers ����Ʈ�� ������ GameObject�� �����ϴ� BaseCharacter�� ���� ��쿡�� �߰�
                    if (!Receivers.Any(e => e.gameObject == ally.gameObject))
                    {
                        Receivers.Add(ally);
                    }
                }
                else
                {
                    // Size�� 1�� Ally�� �׳� �߰�
                    Receivers.Add(ally);
                }
                Receivers.Add(allyFormation[i]);
            }
            else if (4 <= i && i < 8 && currentSelectedSkill.SkillRadius[i])
            {
                BaseCharacter enemy = enemyFormation[i - 4];
                if(enemy == null) continue;

                //���� Size�� 2�� ���
                if(enemy.Size == 2)
                {
                    // �̹� Receivers ����Ʈ�� ������ GameObject�� �����ϴ� BaseCharacter�� ���� ��쿡�� �߰�
                    if (!Receivers.Any(e => e.gameObject == enemy.gameObject))
                    {
                        Receivers.Add(enemy);
                    }
                }
                else
                {
                    // Size�� 1�� ���� �׳� �߰�
                    Receivers.Add(enemy);
                }

            }
        }

        if (Caster != null && Receivers.Count > 0)
        {
            StartCoroutine(ExecuteSkill(Caster, Receivers));
        }
        else
        {
            Debug.LogError("Caster or Enemy not assigned!");
        }
    }

    // ��ų ���� ���� ����
    IEnumerator ExecuteSkill(BaseCharacter _caster, List<BaseCharacter> _receivers)
    {
        foreach(BaseCharacter receiver in _receivers)
        {
            currentSelectedSkill.ApplySkill(receiver);
            if (receiver.CheckDead())
            {
                if (receiver.IsAlly)
                {
                    for (int i = 0; i < allyFormation.Length; i++)
                    {
                        if (allyFormation[i] != null && allyFormation[i] == receiver)
                        {
                            allyFormation[i] = null; // �ڱ� �ڽ��� null�� ����
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < enemyFormation.Length; i++)
                    {
                        if (enemyFormation[i] != null && enemyFormation[i] == receiver)
                        {
                            enemyFormation[i] = null;
                        }
                    }

                }
            };
            Debug.Log(currentSelectedSkill.Name + " is executed by " + _caster.name + " on " + receiver.name);
        }
        
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
        CheckPostBuffs();
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

    void CheckPostBuffs()
    {
        int characterCount = combatQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue���� �׸��� ����
            BaseCharacter character = combatQueue.Dequeue();
            character.ApplyBuff(BuffTiming.RoundEnd);

            // ������ character�� Queue�� ���ʿ� �ٽ� �߰��մϴ�.
            combatQueue.Enqueue(character);
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
        int index = 0;
        BaseCharacter[] formation;

        if (_character.IsAlly)
            formation = allyFormation;
        else
            formation = enemyFormation;
        
        for(int i = 0; i < 4; i++)
        {
            if (formation[i] == null)
            {
                continue;
            }

            if (formation[i] == _character)
            {
                index = i;
            }
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
            if (allyFormation[index] != null)
            {
                return true;
            }
        }
        else if(index < 8)
        {
            if (enemyFormation[index - 4] != null)
            {
                return true;
            }
        }

        return false;
    }
    #region Getter Setter

    public BaseCharacter[] AllyFormation
    {
        get { return allyFormation; }
        private set { allyFormation = value; } 
    }

    public BaseCharacter[] EnemyFormation
    {
        get { return enemyFormation; }
        private set { enemyFormation = value; }
    }
    public BaseSkill CurrentSelectedSkill => currentSelectedSkill;
    #endregion
}
