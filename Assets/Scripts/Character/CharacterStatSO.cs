using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStat_", menuName = "Scriptable Objects/Character/CharacterStat")]
public class CharacterStatSO : ScriptableObject
{

    #region Header CHARACTER BASICS

    [Space(10)]
    [Header("Character Basics")]

    #endregion Header CHARACTER BASICS

    [SerializeField] private string characterName;
    [SerializeField] private int baseHealth;

    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS
    [SerializeField] private int baseSpeed;
    [SerializeField] private int baseDefense;
    [SerializeField] private int baseCrit;
    [SerializeField] private int baseAccuracy;
    [SerializeField] private int baseEvasion;
    [SerializeField] private int baseResist;


    #region Validation
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this,nameof(characterName), characterName);
        HelperUtilities.ValidateCheckOver100(this, nameof(baseDefense), baseDefense);
        HelperUtilities.ValidateCheckOver100(this, nameof(baseCrit), baseCrit);
        HelperUtilities.ValidateCheckOver100(this, nameof(baseAccuracy), baseAccuracy);
        HelperUtilities.ValidateCheckOver100(this, nameof(baseEvasion), baseEvasion);
        HelperUtilities.ValidateCheckOver100(this, nameof(baseResist), baseResist);
    }
    #endregion
}
