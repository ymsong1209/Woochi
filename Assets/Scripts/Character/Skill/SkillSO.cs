using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName = "Scriptable Objects/Character/Skill")]
public class SkillSO : ScriptableObject
{

    #region Header SKILL BASICS

    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL BASICS

    [SerializeField] private string             skillName;

    /// <summary>
    /// 스킬을 사용할 수 있는 열
    /// 0~4 : 아군 1~4열
    /// 5~8 : 적군 1~4열
    /// </summary>
    [SerializeField] private bool[]             skillAvailableRadius = new bool[8];

    /// <summary>
    /// 스킬을 적용시킬 수 있는 열
    /// 0~4 : 아군 1~4열
    /// 5~8 : 적군 1~4열
    /// </summary>
    [SerializeField] private bool[]             skillRadius = new bool[8];    
    [SerializeField] private SkillType          skillType;
    [SerializeField] private SkillTargetType    skillTargetType;
    /// <summary>
    /// 스킬 적중시 적용시킬 버프 리스트
    /// </summary>
    public List<GameObject>           bufflist = new List<GameObject>();       

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private float baseMultiplier;        // 피해량 계수
    [SerializeField] private float baseSkillAccuracy;     // 스킬 명중 수치

    #region Getter Setter
    public string SkillName => skillName;

    public bool[] SkillAvailableRadius => skillAvailableRadius;
    public bool[] SkillRadius => skillRadius;

    public SkillType SkillType => skillType;
    public SkillTargetType SkillTargetType => skillTargetType;
    public float BaseMultiplier => baseMultiplier;

    public float BaseSkillAccuracy => baseSkillAccuracy;
    #endregion

}
