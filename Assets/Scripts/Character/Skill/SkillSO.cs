using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSO : ScriptableObject
{

    #region Header SKILL BASICS

    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL BASICS

    [SerializeField] private string skillName;
    [SerializeField] private SkillRadius skillRadius = SkillRadius.SingularAlly;
    [SerializeField] private SkillType skillType = SkillType.Attack;

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private int baseMinStat;   // 최소 피해 계수
    [SerializeField] private int baseMaxStat;   // 최대 피해 계수
    [SerializeField] private int baseMultStat;  // 피해량 계수

}
