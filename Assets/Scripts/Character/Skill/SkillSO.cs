using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSO : ScriptableObject
{

    #region Header SKILL BASICS

    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL BASICS

    [SerializeField] private string             skillName;
    [SerializeField] private SkillRadius        skillRadius;    
    [SerializeField] private SkillType          skillType;

    /// <summary>
    /// ��ų ���߽� �����ų ���� ����Ʈ
    /// </summary>
    public List<GameObject>           bufflist;       

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private float baseMinStat;       // �ּ� ���
    [SerializeField] private float baseMaxStat;       // �ִ� ���
    [SerializeField] private float baseMultiplier;    // ���ط� ���
    [SerializeField] private float baseSkillAccuracy;     // ��ų ���� ��ġ

    #region Getter Setter
    public SkillRadius SkillRadius => skillRadius;

    public SkillType SkillType => skillType;
    public float BaseMinStat => baseMinStat;
    public float BaseMaxStat => baseMaxStat;
    public float BaseMultiplier => baseMultiplier;

    public float BaseSkillAccuracy => baseSkillAccuracy;
    #endregion

}
