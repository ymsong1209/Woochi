using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아군 캐릭터 정보와 관련된 UI들을 관리하는 클래스
/// 캐릭터 초상화, 이름, 스탯 등을 표시
/// </summary>
public class AllyCharacterUI : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("Stat")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private AllyStat accuracy;
    [SerializeField] private AllyStat evasion;
    [SerializeField] private AllyStat critical;
    [SerializeField] private AllyStat damage;
    [SerializeField] private AllyStat speed;
    [SerializeField] private AllyStat depense;
    [SerializeField] private AllyStat resist;

    void Start()
    {
        #region 이벤트 등록
        BattleManager.GetInstance.OnCharacterTurnStart += ShowCharacterUI;
        BattleManager.GetInstance.OnCharacterAttacked += ShowCharacterUI;
        #endregion
    }

    public void ShowCharacterUI(BaseCharacter _character, bool isEnable = true)
    {
        if(!_character.IsAlly)
            return;

        ShowCharacterInfo(_character);
        ShowCharacterStat(_character);
    }

    /// <summary>
    /// 캐릭터 정보를 조작 UI에 표시
    /// </summary>
    /// <param name="_character"></param>
    private void ShowCharacterInfo(BaseCharacter _character)
    {
        characterName.text = _character.characterStat.CharacterName;
    }

    /// <summary>
    /// 캐릭터 스탯을 UI에 표시
    /// </summary>
    /// <param name="_character"></param>
    private void ShowCharacterStat(BaseCharacter _character)
    {
        hpText.text = $"{_character.Health.CurHealth} / {_character.Health.MaxHealth}";
        accuracy.SetText(_character.Accuracy, _character.ChangedAccuracy);
        critical.SetText(_character.Crit, _character.ChangedCrit, true);
        damage.SetDamageText(_character.MinStat, _character.MaxStat);
        depense.SetText(_character.Defense, _character.ChangedDefense, true);
        evasion.SetText(_character.Evasion, _character.ChangedEvasion);
        resist.SetText(_character.Resist, _character.ChangedResist);
        speed.SetText(_character.Speed, _character.ChangedSpeed);
    }

}
