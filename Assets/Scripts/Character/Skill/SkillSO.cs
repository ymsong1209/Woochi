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

    [SerializeField] private int baseMinStat;       // �ּ� ���
    [SerializeField] private int baseMaxStat;       // �ִ� ���
    [SerializeField] private int baseMultiplier;    // ���ط� ���
    [SerializeField] private int baseSkillAccuracy;     // ��ų ���� ��ġ

    #region Getter Setter
    public SkillRadius SkillRadius => skillRadius;

    public SkillType SkillType => skillType;
    public int BaseMinStat => baseMinStat;
    public int BaseMaxStat => baseMaxStat;
    public int BaseMultiplier => baseMultiplier;

    public int BaseSkillAccuracy => baseSkillAccuracy;
    #endregion

}
