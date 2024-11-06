using System.Collections.Generic;
using UnityEngine;
using DataTable;
using UnityEngine.Serialization;

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
    public Sprite[] icons = new Sprite[3];

    [ReadOnly] public string characterName;
    [ReadOnly] public int size;
    [ReadOnly] public int cost;

    #region Header CHARACTER STATS

    [FormerlySerializedAs("baseStatClass")]
    [Space(10)]
    [Header("Character Stats")]

    #endregion Header CHARACTER STATS    // 보상으로 얻은 스탯
    [SerializeField] private Stat baseStat;
    [SerializeField] private Stat levelUpStat;
    [SerializeField] private Stat rewardStat;
    [SerializeField] private Level level;
    [SerializeField] private Health baseHealth;

    #region Header CHARACTER SKILLS

    [Space(10)]
    [Header("Character Skills")]
    #endregion Header CHARACTER SKILLS
    [SerializeField] private List<BaseSkill> skills;    // 캐릭터가 가지고 있는 스킬 리스트
    [SerializeField] private List<BaseSkill> reinforcedSkills; //강화된 스킬 리스트

    public void Initialize()
    {
        CharacterData data = CharacterData.GetDictionary()[ID];

        characterName = data.characterName;
        size = data.size;
        cost = data.cost;

        CharacterInfoData info = DataCloud.playerData.LoadInfo(ID);
        if(info != null)
        {
            baseStat = new Stat(info.baseStat);
            levelUpStat = new Stat(info.levelUpStat);
            rewardStat = new Stat(info.rewardStat);
            level = new Level(info.level);
            baseHealth = new Health(info.health);
        }
        else
        { 
            baseStat = new Stat(data, false);
            levelUpStat = new Stat(data, true);
            rewardStat = new Stat();
            baseStat = new Stat(data, false);
            level = new Level();
            baseHealth = new Health(data);
        }
    }

    #region Getter Method
    public Stat BaseStat => baseStat;
    public Stat LevelUpStat => levelUpStat;
    public Stat RewardStat => rewardStat;
    public Level Level => level;
    public Health BaseHealth => baseHealth;
    public List<BaseSkill> Skills => skills;

    public List<BaseSkill> ReinforcedSkills => reinforcedSkills;

    #endregion

}
