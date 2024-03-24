using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

public class BattleManager : SingletonMonobehaviour<BattleManager>
{
   
    private BattleState         CurState;
    private GameObject          currentCharacterGameObject;     //���� ���� ��������
    private int                 currentRound;                   //���� �� ��������

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
    [SerializeField] private Queue<GameObject> combatQueue = new Queue<GameObject>();
    [SerializeField, ReadOnly] private GameObject[] allyFormation = new GameObject[4];
    [SerializeField, ReadOnly] private GameObject[] EnemyFormation = new GameObject[4];

    private void Start()
    {
        CurState = BattleState.IDLE;
    }

    /// <summary>
    /// DungeonInfoSO ������ �޾ƿͼ� �Ʊ��� ���� ��ġ�� ����
    /// </summary>
    public void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        currentRound = 0;
        combatQueue.Clear();
        for(int i = 0;i<allyFormation.Length; ++i)
        {
            allyFormation[i] = null;
        }
        for(int i = 0;i<EnemyFormation.Length; ++i)
        {
            EnemyFormation[i] = null;
        }

        #region �Ʊ��� ���� ��ġ
        #region ���� ��ġ
        // DungeonInfo���� �ִ� ������ �˸��� ���� ��ġ
        int EnemyTotalSize = 0;
        for(int i = 0;i<dungeon.EnemyList.Count; ++i)
        {
            GameObject enemyGameObject = dungeon.EnemyList[i];
            BaseCharacter enemyCharacter = enemyGameObject.GetComponent<BaseCharacter>();
            
            //���� ������ ����
            combatQueue.Enqueue(enemyGameObject);
            EnemyFormation[i] = enemyGameObject;

            //��ġ���� ���ؾ��ϴ� Ư���� ��ü�� �ƴϸ� enemyPosition��� ���� ��ȯ
            int enemySize = enemyCharacter.Size;
            if (enemyCharacter.IsSpawnSpecific == false)
            {
                //ũ�Ⱑ 1�� ����
                if (enemySize == 1)
                {
                    Instantiate(enemyGameObject, enemySinglePosition[EnemyTotalSize], Quaternion.identity);
                    ++EnemyTotalSize;
                }
                //ũ�Ⱑ 2�� ����
                else if (enemySize == 2) {
                    Instantiate(enemyGameObject, enemyMultiplePosition[EnemyTotalSize], Quaternion.identity);
                    EnemyTotalSize += 2;
                }
            }
            //��ġ�� ��������ϴ� Ư���� ��ü
            else
            {
                //ũ�Ⱑ 1�� ����
                if (enemySize == 1)
                {
                    Instantiate(enemyGameObject, enemyCharacter.SpawnLocation, enemyCharacter.SpawnRotation);
                    ++EnemyTotalSize;
                }
                //ũ�Ⱑ 2�� ����
                else if (enemySize == 2)
                {
                    Instantiate(enemyGameObject, enemyCharacter.SpawnLocation, enemyCharacter.SpawnRotation);
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
            GameObject allyGameObject = GameManager.GetInstance.Allies[i];
            BaseCharacter allyCharacter = allyGameObject.GetComponent<BaseCharacter>();

            //���� ������ ����
            combatQueue.Enqueue(allyGameObject);
            EnemyFormation[i] = allyGameObject;

            //��ġ���� ���ؾ��ϴ� Ư���� ��ü�� �ƴϸ� allyPosition��� ���� ��ȯ
            int allySize = allyCharacter.Size;
            if (allyCharacter.IsSpawnSpecific == false)
            {
                //ũ�Ⱑ 1�� �Ʊ�
                if (allySize == 1)
                {
                    Instantiate(allyGameObject, allySinglePosition[AllyTotalSize], Quaternion.identity);
                    ++AllyTotalSize;
                }
                //ũ�Ⱑ 2�� �Ʊ�
                else if (allySize == 2)
                {
                    Instantiate(allyGameObject, allyMultiplePosition[AllyTotalSize], Quaternion.identity);
                    AllyTotalSize += 2;
                }
            }
            //��ġ�� ��������ϴ� Ư���� ��ü
            else
            {
                //ũ�Ⱑ 1�� �Ʊ�
                if (allySize == 1)
                {
                    Instantiate(allyGameObject, allyCharacter.SpawnLocation, allyCharacter.SpawnRotation);
                    ++AllyTotalSize;
                }
                //ũ�Ⱑ 2�� �Ʊ�
                else if (allySize == 2)
                {
                    Instantiate(allyGameObject, allyCharacter.SpawnLocation, allyCharacter.SpawnRotation);
                    AllyTotalSize += 2;
                }
            }
        }
        #endregion �Ʊ� ��ġ
        #endregion �Ʊ��� ���� ��ġ

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
            GameObject characterGameObject = combatQueue.Dequeue();
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundStartBuff();
                //���� üũ
                character.CheckDead();
            }

            // ������ character�� Queue�� ���ʿ� �ٽ� �߰�.
            combatQueue.Enqueue(characterGameObject);
        }

    }

    /// <summary>
    /// ĳ���͵��� �ӵ������� ����
    /// </summary>
    void DetermineOrder()
    {
        CurState = BattleState.DetermineOrder;
        SortBattleOrder();
        CharacterTurn();
    }

    /// <summary>
    /// ĳ���͵��� �ӵ������� ����
    /// </summary>
    void SortBattleOrder()
    {
        List<GameObject> SortList = new List<GameObject>();

        // Queue�� �ִ� ��� �ϳ��� ������ List�� �������
        foreach (GameObject character in combatQueue)
        {
            SortList.Add(character);
        }

        // List ������ characters�� Speed �Ӽ��� �������� �������� ����
        SortList.Sort((character1, character2) => character2.GetComponent<BaseCharacter>().Speed.CompareTo(character1.GetComponent<BaseCharacter>().Speed));

        // Queue�� ����
        combatQueue.Clear();

        // ���ĵ� List ������ characters�� �ٽ� combatQueue�� �������
        foreach (GameObject character in SortList)
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
        //ĳ���ͺ��� �ൿ
        StartCoroutine(HandleCharacterTurns());
       
    }

    IEnumerator HandleCharacterTurns()
    {
        List<GameObject> processedCharacters = new List<GameObject>();

        while (combatQueue.Count > 0)
        {
            currentCharacterGameObject = combatQueue.Dequeue();
            BaseCharacter currentCharacter = currentCharacterGameObject.GetComponent<BaseCharacter>();

            if (currentCharacter.IsDead)
            {
                processedCharacters.Add(currentCharacterGameObject);
                continue;
            }

            // ���ʰ� ���� �� ���� ����
            currentCharacter.ApplyTurnStartBuffs();

            yield return StartCoroutine(WaitForSkillSelection(currentCharacter));

            // ��ų ������� ���� �ӵ� ���� ó��
            ReordercombatQueue(processedCharacters);

            processedCharacters.Add(currentCharacterGameObject);

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


            yield return new WaitForSeconds(1f); // ��ų �ִϸ��̼� ���� ���� ��� �ð�
        }

        //��� ĳ������ ���� ������ �� ����
        PostRound();
    }

    /// <summary>
    /// �ӽ÷� ���� �Լ�. �뷮 ���� �ʿ�
    /// </summary>
    IEnumerator WaitForSkillSelection(BaseCharacter character)
    {
        bool skillSelected = false;
       
        BaseSkill selectedSkill = null;
        int selectedSkillIndex = -1;

        // ��ų ������ ���� �ӽ� UI �޽���
        Debug.Log("Press 1 or 2 to select a skill for " + character.name);

        while (!skillSelected)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // ���⼭�� ���÷� ù ��° ��ų�� �����ߴٰ� ����.
                Debug.Log("Skill 1 selected");
                selectedSkill = character.skills[0];
                selectedSkillIndex = 0;
                skillSelected = true;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // �� ��° ��ų�� ���õǾ��ٰ� ����.
                Debug.Log("Skill 2 selected");
                selectedSkill = character.skills[1];
                selectedSkillIndex = 1;
                skillSelected = true;
            }

            yield return null; // ���� �����ӱ��� ���
        }

        // ���õ� ��ų�� ����ϴ� ����
        if (selectedSkill != null && selectedSkillIndex != -1)
        {
            // ������ 0��° ĳ���Ϳ��� ��ų�� ���ٰ� ����
            BaseCharacter temporaryEnemy = EnemyFormation[0].GetComponent<BaseCharacter>();
            BaseCharacter temporaryCaster = allyFormation[0].GetComponent<BaseCharacter>();

            yield return StartCoroutine(ExecuteSkill(selectedSkillIndex, temporaryCaster, temporaryEnemy));
        }
    }

    // ��ų ���� ���� ����
    IEnumerator ExecuteSkill(int _skillindex, BaseCharacter _caster, BaseCharacter _receiver)
    {
        BaseSkill skill = _caster.skills[_skillindex];

        yield return new WaitForSeconds(1f); // ���÷� 1�� ���
    }

    void ReordercombatQueue(List<GameObject> processedCharacters)
    {
        List<GameObject> allCharacters = new List<GameObject>(processedCharacters);

        // combatQueue�� ���� �ִ� ĳ���͸� ��� allCharacters ����Ʈ�� �߰�
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters ����Ʈ�� �ӵ��� ���� ������
        allCharacters.Sort((character1, character2) => character2.GetComponent<BaseCharacter>().Speed.CompareTo(character1.GetComponent<BaseCharacter>().Speed));

        // �����ĵ� ����Ʈ�� �������� combatQueue �籸��
        combatQueue.Clear();
        foreach (GameObject character in allCharacters)
        {
            combatQueue.Enqueue(character);
        }
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
            GameObject characterGameObject = combatQueue.Dequeue();
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundEndBuff();
                //���� üũ
                character.CheckDead();
            }

            // ������ character�� Queue�� ���ʿ� �ٽ� �߰��մϴ�.
            combatQueue.Enqueue(characterGameObject);
        }
    }

    /// <summary>
    /// ������ ��� �׾����� Ȯ��
    /// </summary>
    bool CheckVictory(IEnumerable<GameObject> characters)
    {

        foreach (GameObject characterGameObject in characters)
        {
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();
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
    bool CheckDefeat(IEnumerable<GameObject> characters)
    {
        foreach (GameObject characterGameObject in characters)
        {
            BaseCharacter character = characterGameObject.GetComponent<BaseCharacter>();
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
            BaseCharacter curchar = combatQueue.Dequeue().GetComponent<BaseCharacter>();
            if(curchar.IsAlly == false)
            {
                curchar.Destroy();
            }
        }
        combatQueue.Clear();
    }
}
