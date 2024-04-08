using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    // 스킬 툴팁
    public GameObject skillTooltip;
    [SerializeField] private TextMeshProUGUI skillNameTxt;

    // 적 캐릭터 정보 툴팁
    public GameObject enemyTooltip;
    [SerializeField] private TextMeshProUGUI enemyNameTxt;
    [SerializeField] private TextMeshProUGUI enemyEvasionTxt;
    [SerializeField] private TextMeshProUGUI enemySpeedTxt;

    void Start()
    {
        
    }

    public void SetSkillToolTip(BaseSkill _skill, Vector3 position)
    {
        skillTooltip.SetActive(true);
        skillTooltip.transform.position = position;
        skillNameTxt.text = _skill.Name;
    }

    public void SetEnemyToolTip(BaseCharacter _character)
    {
        enemyTooltip.SetActive(true);
        enemyNameTxt.text = _character.name;
        enemyEvasionTxt.text = $"회피 : {_character.Evasion}";
        enemySpeedTxt.text = $"속도 : {_character.Speed}";
    }
}
