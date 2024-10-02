using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// 턴과 관련된 클래스
/// </summary>
public class TurnManager : MonoBehaviour
{
    /// <summary>
    /// 아군이랑 적군의 싸움 순서
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
    /// BuffTiming을 매개변수로 받아서 해당 시점에 버프를 적용
    /// </summary>
    public void CheckBuffs(BuffTiming buffTiming)
    {
        foreach(var character in combatQueue)
        {
            character.TriggerBuff(buffTiming);
        }
    }

    /// <summary>
    /// combatQueue를 다시 속도순으로 정렬, ResetTurnUsed를 true로 하면 모든 캐릭터가 턴을 다시 쓸 수 있음
    /// </summary>
    /// <param name="_resetTurnUsed">true로 설정 시 모든 캐릭터 다시 턴 사용가능</param>
    /// <param name="processedCharacters"></param>
    public void ReorderCombatQueue(bool _resetTurnUsed = false)
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
        allCharacters.Sort((character1, character2) => character2.FinalStat.speed.CompareTo(character1.FinalStat.speed));

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

    public bool IsContinueTurn() => combatQueue.Count > 0;
    
    public void Processed(BaseCharacter character) => processedCharacters.Add(character);

    /// <summary>
    /// 다음 턴 시작되는 캐릭터에게 해야할 처리를 하고 턴을 시작하는 캐릭터 설정
    /// 턴을 진행할 수 없는 캐릭터라면 false 반환
    /// </summary>
    public bool StartTurn()
    {
        UpdateUI();
        BaseCharacter currentCharacter = combatQueue.Dequeue();

        if (currentCharacter.IsDead || currentCharacter.IsTurnUsed || currentCharacter.Health.CheckHealthZero())
        {
            Debug.Log($"{currentCharacter.Name}은(는) 죽거나 턴을 진행할 수 없습니다");
            processedCharacters.Add(currentCharacter);
            return false;
        }

        currentCharacter.HUD.ShowTurnEffect();
        BattleManager.GetInstance.currentCharacter = currentCharacter;
        return true;
    }

    /// <summary>
    /// 캐릭터의 턴이 끝났을 때 해당 캐릭터에게 처리
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
    /// 턴이 완전히 끝났을 때(턴을 진행할 캐릭터가 없음)
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
    /// 정비 시 사용 -> 우치만 전투 큐에 추가
    /// </summary>
    public void OnlyWoochi(BaseCharacter woochi)
    {
        combatQueue.Clear();
        combatQueue.Enqueue(woochi);
    }

    public void UnSummon(BaseCharacter target)
    {
        // 턴 사용한 소환수 경우 -> 처리된 캐릭터 리스트에서 제거
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
        // 턴 사용하지 않은 소환수 경우 -> combatQueue에서 제거
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
    /// 전투가 끝난 후 턴 UI 초기화
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
