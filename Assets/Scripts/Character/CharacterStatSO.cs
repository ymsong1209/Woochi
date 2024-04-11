using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "CS_", menuName = "Scriptable Objects/Character/CharacterStat")]
public class CharacterStatSO : ScriptableObject
{

    #region Header CHARACTER BASICS

    [Space(10)]
    [Header("Character Basics")]

    #endregion Header CHARACTER BASICS

    [SerializeField] private string characterName;


    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS
    [SerializeField] private float    baseHealth;
    [SerializeField] private float    baseSpeed;
    [SerializeField] private float    baseDefense;
    [SerializeField] private float    baseCrit;
    [SerializeField] private float    baseAccuracy;   //����
    [SerializeField] private float    baseEvasion;    //ȸ��
    [SerializeField] private float    baseResist;     //����
    [SerializeField] private float    baseMinStat;
    [SerializeField] private float    baseMaxStat;

    #region Header CHARACTER SKILLS

    [Space(10)]
    [Header("Character Skills")]
    #endregion Header CHARACTER SKILLS
    [SerializeField] private List<BaseSkill> skills;    // ĳ���Ͱ� ������ �ִ� ��ų ����Ʈ



    #region Getter Method
    public string CharacterName => characterName;
    public float BaseHealth => baseHealth;

    public float BaseSpeed => baseSpeed;
    public float BaseDefense => baseDefense;
    public float BaseCrit => baseCrit;
    public float BaseAccuracy => baseAccuracy;
    public float BaseEvasion => baseEvasion;
    public float BaseResist => baseResist;

    public float BaseMinStat => baseMinStat;
    public float BaseMaxStat => baseMaxStat;

    public List<BaseSkill> Skills => skills;
    #endregion


    #region Validation
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this,nameof(characterName), characterName);
        HelperUtilities.ValidateCheckUnderZero(this, nameof(baseHealth), baseHealth);
        HelperUtilities.ValidateRange0To100(this, nameof(baseDefense), baseDefense);
        HelperUtilities.ValidateRange0To100(this, nameof(baseCrit), baseCrit);
        HelperUtilities.ValidateRange0To100(this, nameof(baseAccuracy), baseAccuracy);
        HelperUtilities.ValidateRange0To100(this, nameof(baseEvasion), baseEvasion);
        HelperUtilities.ValidateRange0To100(this, nameof(baseResist), baseResist);

    }
    #endregion
}
