using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [HeaderTooltip("Skill Tooltip","스킬 아이콘에 마우스 올릴 시 뜨는 툴팁")]
    public GameObject skillTooltip;
    [SerializeField] private TextMeshProUGUI skillNameTxt;

    [HeaderTooltip("Enemy Tooltip", "적 캐릭터에 마우스 올릴 시 적 정보 뜨는 툴팁")]
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
        enemyHPTxt.text = $"체력 : {_character.Health.CurHealth} / {_character.Health.MaxHealth}";
        enemyEvasionTxt.text = $"회피 : {_character.Evasion}";
        enemySpeedTxt.text = $"속도 : {_character.Speed}";
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
