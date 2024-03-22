using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.TextCore.Text;

public class BattleManager : SingletonMonobehaviour<GameManager>
{
   
    private BattleState         CurState;
    private BaseCharacter       currentCharacter;   //���� ���� ��������

    /// <summary>
    /// �Ʊ��̶� ������ CharacterList�� ä��
    /// </summary>
    [SerializeField] private Queue<BaseCharacter> CharacterQueue = new Queue<BaseCharacter>();

    private void Update()
    {
        if(CurState == BattleState.CharacterTurn)
        {

        }
    }


    /// <summary>
    /// DungeonInfoSO ������ �޾ƿͼ� �Ʊ��� ���� ��ġ�� ����
    /// </summary>
    void InitializeBattle(DungeonInfoSO dungeon)
    {
        CurState = BattleState.Initialization;
        if (dungeon == null) { Debug.LogError("Null Dungeon"); return; }

        #region CharacterQueue �ʱ�ȭ
        CharacterQueue.Clear();
        #endregion

        #region �Ʊ��� ���� ��ġ
        // DungeonInfo���� �ִ� ������ �˸��� ���� ��ġ
        foreach (BaseCharacter enemy in dungeon.EnemyList)
        {
            CharacterQueue.Enqueue(enemy);
            //Todo : ĳ���͸� �˸��� ���� ��ġ
            Transform trans = enemy.transform;
            Instantiate(enemy,trans);
        }
       
        foreach(BaseCharacter ally in GameManager.GetInstance.Allies)
        {
            CharacterQueue.Enqueue(ally);
            //Todo : �Ʊ� ĳ���� �˸°� ��ġ
        }

        #endregion

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
        CheckPreBuffs();
        DetermineOrder();
    }

    /// <summary>
    /// ���尡 �����Ҷ� ����Ǵ� ���� ����
    /// </summary>
    void CheckPreBuffs()
    {
        int characterCount = CharacterQueue.Count;
        for (int i = 0; i < characterCount; i++)
        {
            // Queue���� �׸��� ����
            BaseCharacter character = CharacterQueue.Dequeue();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundStartBuff();
                //���� üũ
                character.CheckDead();
            }

            // ������ character�� Queue�� ���ʿ� �ٽ� �߰��մϴ�.
            CharacterQueue.Enqueue(character);
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
        List<BaseCharacter> SortList = new List<BaseCharacter>();

        // Queue�� �ִ� ��� �ϳ��� ������ List�� �������
        foreach (BaseCharacter character in CharacterQueue)
        {
            SortList.Add(character);
        }

        // List ������ characters�� Speed �Ӽ��� �������� �������� ����
        SortList.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // Queue�� ����
        CharacterQueue.Clear();

        // ���ĵ� List ������ characters�� �ٽ� CharacterQueue�� �������
        foreach (BaseCharacter character in SortList)
        {
            CharacterQueue.Enqueue(character);
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
        List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

        while (CharacterQueue.Count > 0)
        {
            currentCharacter = CharacterQueue.Dequeue();

            if (currentCharacter.IsDead)
            {
                processedCharacters.Add(currentCharacter);
                continue;
            }

            // ���ʰ� ���� �� ���� ����
            currentCharacter.ApplyTurnStartBuffs();



            yield return StartCoroutine(WaitForSkillSelection(currentCharacter));

            // ��ų ������� ���� �ӵ� ���� ó��
            ReorderCharacterQueue(processedCharacters);

            processedCharacters.Add(currentCharacter);

            yield return new WaitForSeconds(1f); // ��ų �ִϸ��̼� ���� ���� ��� �ð�
        }

        PostRound();
    }

    IEnumerator WaitForSkillSelection(BaseCharacter character)
    {
        // ��ų ���� UI Ȱ��ȭ
        // ��: skillSelectionUI.ShowForCharacter(character);

        bool skillSelected = false;
        BaseSkill selectedSkill = null;

        // ��ų�� ���õ� ������ ���
        // �� �κ��� ���� UI ������ ���� �޶��� �� �ֽ��ϴ�.
        // ���� ���, ��ų ���� ��ư�� Ŭ���Ǹ� �Ʒ��� ���� �Լ��� ȣ���ϵ��� ����
        skillSelectionUI.OnSkillSelected += (skill) =>
        {
            selectedSkill = skill;
            skillSelected = true;
        };

        // ��ų�� ���õ� ������ ���� ���
        yield return new WaitUntil(() => skillSelected);

        // ���õ� ��ų ����
        if (selectedSkill != null)
        {
            yield return StartCoroutine(ExecuteSkill(selectedSkill, character));
        }

        // ��ų ���� UI ��Ȱ��ȭ
        // ��: skillSelectionUI.Hide();
    }

    IEnumerator ExecuteSkill(BaseSkill skill, BaseCharacter character)
    {
        // ��ų ���� ���� ����
        // ��: �ִϸ��̼� ���, ����� ��� ��
        yield return new WaitForSeconds(1f); // ���÷� 1�� ���
    }

    void ReorderCharacterQueue(List<BaseCharacter> processedCharacters)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>(processedCharacters);

        // CharacterQueue�� ���� �ִ� ĳ���͸� ��� allCharacters ����Ʈ�� �߰�
        while (CharacterQueue.Count > 0)
        {
            allCharacters.Add(CharacterQueue.Dequeue());
        }

        // allCharacters ����Ʈ�� �ӵ��� ���� ������
        allCharacters.Sort((character1, character2) => character2.Speed.CompareTo(character1.Speed));

        // �����ĵ� ����Ʈ�� �������� CharacterQueue �籸��
        CharacterQueue.Clear();
        foreach (BaseCharacter character in allCharacters)
        {
            CharacterQueue.Enqueue(character);
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
            // Queue���� �׸��� ����
            BaseCharacter character = CharacterQueue.Dequeue();

            foreach (BaseBuff buff in character.activeBuffs)
            {
                buff.ApplyRoundEndBuff();
                //���� üũ
                character.CheckDead();
            }

            // ������ character�� Queue�� ���ʿ� �ٽ� �߰��մϴ�.
            CharacterQueue.Enqueue(character);
        }
    }

    /// <summary>
    /// ������ ��� �׾����� Ȯ��
    /// </summary>
    bool VictoryCheck()
    {
        bool Victory = true;
        // Queue�� �ִ� ��� �ϳ��� ������ List�� �������
        foreach (BaseCharacter character in CharacterQueue)
        {
            if (Victory == false) continue;
            //������ ���
            if(character.IsAlly == false)
            {
                //������ ������� ���
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
    /// �Ʊ��� ��� �׾����� Ȯ��
    /// </summary>
    bool DefeatCheck()
    {
        bool Defeat = true;
        // Queue�� �ִ� ��� �ϳ��� ������ List�� �������
        foreach (BaseCharacter character in CharacterQueue)
        {
            if (Defeat == false) continue;
            //�Ʊ��� ���
            if (character.IsAlly )
            {
                //�Ʊ��� ������� ���
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
    /// ���� ���� ��, ���� ����
    /// </summary>
    void PostBattle()
    {

    }
}
