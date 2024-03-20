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
    [SerializeField] private BaseBuff           buff;

    #region Header SKILL STATS
    [Space(10)]
    [Header("Skill Basics")]

    #endregion Header SKILL STATS

    [SerializeField] private int baseMinStat;       // 최소 계수
    [SerializeField] private int baseMaxStat;       // 최대 계수
    [SerializeField] private int baseMultiplier;    // 피해량 계수

}
