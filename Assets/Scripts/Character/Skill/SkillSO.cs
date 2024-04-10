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
    /// ��ų�� ����� �� �ִ� ��
    /// 0~4 : �Ʊ� 1~4��
    /// 5~8 : ���� 1~4��
    /// </summary>
    [SerializeField] private bool[]             skillAvailableRadius = new bool[8];

    /// <summary>
    /// ��ų�� �����ų �� �ִ� ��
    /// 0~4 : �Ʊ� 1~4��
    /// 5~8 : ���� 1~4��
    /// </summary>
    [SerializeField] private bool[]             skillRadius = new bool[8];    
    [SerializeField] private SkillType          skillType;
    [SerializeField] private SkillTargetType    skillTargetType;
    /// <summary>
    /// ��ų ���߽� �����ų ���� ����Ʈ
    /// </summary>
    public List<GameObject>           bufflist = new List<GameObject>();       

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private float baseMultiplier;        // ���ط� ���
    [SerializeField] private float baseSkillAccuracy;     // ��ų ���� ��ġ

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
