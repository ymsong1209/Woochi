using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �Ʊ� ĳ���� ������ ���õ� UI���� �����ϴ� Ŭ����
/// ĳ���� �ʻ�ȭ, �̸�, ���� ���� ǥ��
/// </summary>
public class AllyCharacterUI : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private Image characterPortrait;
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
        #region �̺�Ʈ ���
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
    /// ĳ���� ������ ���� UI�� ǥ��
    /// </summary>
    /// <param name="_character"></param>
    private void ShowCharacterInfo(BaseCharacter _character)
    {
        // ĳ���� �ʻ�ȭ ��������Ʈ�� ���� ��� �����ؾ� �� �� �����̶� �ּ� ó��
        // ��� ����
        // 1. CharacterStatSO�� ĳ���� ���õ� �͵��� �����ϴ� ������ ���ٸ� ���⿡ �����ϰ� BaseCharacter���� ������ �� �ְ� �ϴ°� ���
        // 2. �ƴϸ� ���� ���� ������ ���� �ű⿡ �����ϰ� ������ �� �ְ� �ϴ� ���
        // characterPortrait.sprite = _character.CharacterPortrait;

        characterName.text = _character.characterStat.CharacterName;
    }

    /// <summary>
    /// ĳ���� ������ UI�� ǥ��
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
