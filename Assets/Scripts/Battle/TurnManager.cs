using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// �ϰ� ���õ� Ŭ����
/// </summary>
public class TurnManager : MonoBehaviour
{
    /// <summary>
    /// �Ʊ��̶� ������ �ο� ����
    /// </summary>
    private Queue<BaseCharacter> combatQueue = new Queue<BaseCharacter>();
    private List<BaseCharacter> processedCharacters = new List<BaseCharacter>();

    [Header("UI")]
    [SerializeField] private TurnIcon[] turnIcons;
    [SerializeField] private TextMeshProUGUI roundTxt;
    [SerializeField] private Sprite emptyIcon;

    public void Init(Formation allies, Formation enemies)
    {
        combatQueue.Clear();    processedCharacters.Clear();

        foreach (Formation form in new Formation[] { allies, enemies })
        {
            for (int index = 0; index < form.formation.Length;)
            {
                if (form.formation[index] == null) break;
                combatQueue.Enqueue(form.formation[index]);
                index += form.formation[index].Size;
            }
        }
    }

    /// <summary>
    /// BuffTiming�� �Ű������� �޾Ƽ� �ش� ������ ������ ����
    /// </summary>
    public void CheckBuffs(BuffTiming buffTiming)
    {
        foreach(var character in combatQueue)
        {
            character.TriggerBuff(buffTiming);
        }
    }

    /// <summary>
    /// combatQueue�� �ٽ� �ӵ������� ����, ResetTurnUsed�� true�� �ϸ� ��� ĳ���Ͱ� ���� �ٽ� �� �� ����
    /// </summary>
    /// <param name="_resetTurnUsed">true�� ���� �� ��� ĳ���� �ٽ� �� ��밡��</param>
    /// <param name="processedCharacters"></param>
    public void ReorderCombatQueue(bool _resetTurnUsed = false)
    {
        List<BaseCharacter> allCharacters = new List<BaseCharacter>();

        if (_resetTurnUsed && processedCharacters != null)
        {
            allCharacters.AddRange(processedCharacters);
        }

        // combatQueue�� ���� �ִ� ĳ���͸� ��� allCharacters ����Ʈ�� �߰�
        while (combatQueue.Count > 0)
        {
            allCharacters.Add(combatQueue.Dequeue());
        }

        // allCharacters ����Ʈ�� �ӵ��� ���� ������
        allCharacters.Sort((character1, character2) => character2.FinalStat.speed.CompareTo(character1.FinalStat.speed));

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

    public bool IsContinueTurn() => combatQueue.Count > 0;
    
    public void Processed(BaseCharacter character) => processedCharacters.Add(character);

    /// <summary>
    /// ���� �� ���۵Ǵ� ĳ���Ϳ��� �ؾ��� ó���� �ϰ� ���� �����ϴ� ĳ���� ����
    /// ���� ������ �� ���� ĳ���Ͷ�� false ��ȯ
    /// </summary>
    public bool StartTurn()
    {
        UpdateUI();
        BaseCharacter currentCharacter = combatQueue.Dequeue();

        if (currentCharacter.IsDead || currentCharacter.IsTurnUsed || currentCharacter.Health.CheckHealthZero())
        {
            Debug.Log($"{currentCharacter.Name}��(��) �װų� ���� ������ �� �����ϴ�");
            processedCharacters.Add(currentCharacter);
            return false;
        }

        currentCharacter.HUD.ShowTurnEffect();
        BattleManager.GetInstance.currentCharacter = currentCharacter;
        return true;
    }

    /// <summary>
    /// ĳ������ ���� ������ �� �ش� ĳ���Ϳ��� ó��
    /// </summary>
    /// <param name="currentCharacter"></param>
    public void EndTurn(BaseCharacter currentCharacter)
    {
        currentCharacter.IsTurnUsed = true;
        currentCharacter.TriggerBuff(BuffTiming.TurnEnd);
        processedCharacters.Add(currentCharacter);
        ReorderCombatQueue(false);
    }

    /// <summary>
    /// ���� ������ ������ ��(���� ������ ĳ���Ͱ� ����)
    /// </summary>
    public void TurnOver()
    {
        foreach(var character in processedCharacters)
        {
            combatQueue.Enqueue(character);
        }

        processedCharacters.Clear();
    }

    /// <summary>
    /// ���� �� ��� -> ��ġ�� ���� ť�� �߰�
    /// </summary>
    public void OnlyWoochi(BaseCharacter woochi)
    {
        combatQueue.Clear();
        combatQueue.Enqueue(woochi);
    }

    public void UnSummon(BaseCharacter target)
    {
        // �� ����� ��ȯ�� ��� -> ó���� ĳ���� ����Ʈ���� ����
        if (target.IsTurnUsed)
        {
            foreach(var character in processedCharacters)
            {
                if(character == target)
                {
                    processedCharacters.Remove(character);
                    break;
                }
            }
        }
        // �� ������� ���� ��ȯ�� ��� -> combatQueue���� ����
        else
        {
            Queue<BaseCharacter> tempQueue = new Queue<BaseCharacter>();

            while (combatQueue.Count > 0)
            {
                BaseCharacter character = combatQueue.Dequeue();
                if (character == target)
                {
                    continue;
                }
                tempQueue.Enqueue(character);
            }

            combatQueue = tempQueue;
        }
    }

    /// <summary>
    /// ������ ���� �� �� UI �ʱ�ȭ
    /// </summary>
    public void BattleOver()
    {
        foreach (var icon in turnIcons)
        {
            icon.SetEmpty(emptyIcon);
        }

        roundTxt.text = "0";
    }

    public void SetRound(int round) => roundTxt.text = $"{round}";

    private void UpdateUI()
    {
        int order = 0;
        foreach (var character in combatQueue)
        {
            if (order >= turnIcons.Length) return;
            if(character.IsDead || character.Health.CheckHealthZero())
            {
                continue;
            }

            turnIcons[order].SetIcon(character, order == 0);
            order++;
        }

        for(int i = order; i < turnIcons.Length; i++)
        {
            turnIcons[i].SetEmpty(emptyIcon);
        }
    }
}
