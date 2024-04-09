using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    // ��ų ����
    public GameObject skillTooltip;
    [SerializeField] private TextMeshProUGUI skillNameTxt;

    // �� ĳ���� ���� ����
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
        enemyEvasionTxt.text = $"ȸ�� : {_character.Evasion}";
        enemySpeedTxt.text = $"�ӵ� : {_character.Speed}";
    }
}
