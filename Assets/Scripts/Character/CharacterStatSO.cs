using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStat_", menuName = "Scriptable Objects/Character/CharacterStat")]
public class CharacterStatSO : ScriptableObject
{

    #region Header CHARACTER BASICS

    [Space(10)]
    [Header("Character Basics")]

    #endregion Header CHARACTER BASICS

    [SerializeField] private string characterName;
    [SerializeField] private int    baseHealth = 1;
    [SerializeField] private int    size = 1;

    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS

    [SerializeField] private int    baseSpeed;
    [SerializeField] private int    baseDefense;
    [SerializeField] private int    baseCrit;
    [SerializeField] private int    baseAccuracy;
    [SerializeField] private int    baseEvasion;
    [SerializeField] private int    baseResist;
   

    #region Header SpecializedStats
    [Tooltip("특정 위치에서 Spawn되게 하고 싶으면 값 입력.")]
    [SerializeField] private Vector3 spawnLocation;
    #endregion


    #region Getter Method
    public string CharacterName => characterName;
    public int BaseHealth => baseHealth;
    public int Size => size;
    public int BaseSpeed => baseSpeed;
    public int BaseDefense => baseDefense;
    public int BaseCrit => baseCrit;
    public int BaseAccuracy => baseAccuracy;
    public int BaseEvasion => baseEvasion;
    public int BaseResist => baseResist;
    public Vector3 SpawnLocation => spawnLocation;
    #endregion


    #region Validation
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this,nameof(characterName), characterName);
        HelperUtilities.ValidateCheckUnderZero(this, nameof(baseHealth), baseHealth);
        HelperUtilities.ValidateCheckUnderZero(this, nameof(Size), Size);
        HelperUtilities.ValidateRange0To100(this, nameof(baseDefense), baseDefense);
        HelperUtilities.ValidateRange0To100(this, nameof(baseCrit), baseCrit);
        HelperUtilities.ValidateRange0To100(this, nameof(baseAccuracy), baseAccuracy);
        HelperUtilities.ValidateRange0To100(this, nameof(baseEvasion), baseEvasion);
        HelperUtilities.ValidateRange0To100(this, nameof(baseResist), baseResist);
        
    }
    #endregion
}
