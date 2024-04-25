using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [HeaderTooltip("Skill Tooltip","��ų �����ܿ� ���콺 �ø� �� �ߴ� ����")]
    public GameObject skillTooltip;
    [SerializeField] private TextMeshProUGUI skillNameTxt;

    [HeaderTooltip("Enemy Tooltip", "�� ĳ���Ϳ� ���콺 �ø� �� �� ���� �ߴ� ����")]
    public GameObject enemyTooltip;
    [SerializeField] private TextMeshProUGUI enemyNameTxt;
    [SerializeField] private TextMeshProUGUI enemyHPTxt;
    [SerializeField] private TextMeshProUGUI enemyEvasionTxt;
    [SerializeField] private TextMeshProUGUI enemySpeedTxt;

    [SerializeField] private AllyCharacterUI allyCharacterUI;
    public void SetSkillToolTip(BaseSkill _skill, Vector3 position)
    {
        skillTooltip.SetActive(true);
        skillTooltip.transform.position = position;
        skillNameTxt.text = _skill.Name;
    }

    public void SetEnemyToolTip(BaseCharacter _character)
    {
        enemyTooltip.SetActive(true);
        enemyNameTxt.text = _character.characterStat.CharacterName;
        enemyHPTxt.text = $"ü�� : {_character.Health.CurHealth} / {_character.Health.MaxHealth}";
        enemyEvasionTxt.text = $"ȸ�� : {_character.Evasion}";
        enemySpeedTxt.text = $"�ӵ� : {_character.Speed}";
    }

    public void OnCharacterDamaged(BaseCharacter _character)
    {
        if (_character.IsAlly)
        {

        }
        else
        {
            SetEnemyToolTip(_character);
        }
    }
}
