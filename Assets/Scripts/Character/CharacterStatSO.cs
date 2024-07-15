using System.Collections.Generic;
using UnityEngine;
using DataTable;

[CreateAssetMenu(fileName = "CS_", menuName = "Scriptable Objects/Character/CharacterStat")]
public class CharacterStatSO : ScriptableObject
{
    #region Header CHARACTER BASICS

    [Space(10)]
    [Header("Character Basics")]

    #endregion Header CHARACTER BASICS

    [Tooltip("ID만 입력해주면 나머지는 자동으로 채워집니다.")]
    public int ID;              
    public Sprite portrait;

    private string characterName;
    private int size;
    private int cost;

    #region Header CHARACTER STATS

    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS
    [SerializeField] private Stat baseStat;

    #region Header CHARACTER SKILLS

    [Space(10)]
    [Header("Character Skills")]
    #endregion Header CHARACTER SKILLS
    [SerializeField] private List<BaseSkill> skills;    // 캐릭터가 가지고 있는 스킬 리스트

    public void Initialize()
    {
        CharacterData data = CharacterData.GetDictionary()[ID];

        characterName = data.characterName;
        size = data.size;
        cost = data.cost;
        baseStat = new Stat(data);
    }

    #region Getter Method
    public string CharacterName => characterName;
    public int Size => size;
    public int Cost => cost;
    public Stat BaseStat => baseStat;
    public List<BaseSkill> Skills => skills;
    #endregion

}
