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
    [SerializeField] private int    baseHealth;
    [SerializeField] private int    baseSpeed;
    [SerializeField] private int    baseDefense;
    [SerializeField] private int    baseCrit;
    [SerializeField] private int    baseAccuracy;   //명중
    [SerializeField] private int    baseEvasion;    //회피
    [SerializeField] private int    baseResist;     //저항
   

   


    #region Getter Method
    public string CharacterName => characterName;
    public int BaseHealth => baseHealth;

    public int BaseSpeed => baseSpeed;
    public int BaseDefense => baseDefense;
    public int BaseCrit => baseCrit;
    public int BaseAccuracy => baseAccuracy;
    public int BaseEvasion => baseEvasion;
    public int BaseResist => baseResist;
   
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
