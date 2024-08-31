using System;
using OneLine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "Library_", menuName = "Scriptable Objects/Library")]
public class Library : ScriptableObject
{
    // Warning!. 변수 이름 수정하면 데이터 싹 날라감

    #region Character Prefab
    [OneLineWithHeader, SerializeField] private List<Entry<GameObject>> characters;
    public GameObject GetCharacter(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return characters.FirstOrDefault(entry => entry.ID == id).value;
    }

    public List<GameObject> GetCharacterList(int[] IDs)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (int id in IDs)
        {
            GameObject entry = GetCharacter(id);

            if (entry != null)
                list.Add(entry);
        }

        return list;
    }
    #endregion

    #region Abnormal
    [OneLineWithHeader, SerializeField] private List<Entry<Abnormal>> abnormals;

    public Abnormal GetAbnormal(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return abnormals.FirstOrDefault(entry => entry.ID == id).value;
    }
    #endregion

    #region Reward
    [SerializeField] private RewardList[] rewardLists;

    public Reward GetReward(RareType rarity)
    {
        var list = rewardLists[(int)rarity].rewards;

        return list.Random();
    }

    #endregion
    
    #region Strange
    
    [OneLineWithHeader, SerializeField] private List<Entry<GameObject>> stranges;

    public GameObject GetStrange(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return stranges.FirstOrDefault(entry => entry.ID == id)?.value;
    }
    #endregion

    #region WoochiSkill
    [SerializeField] private List<WoochiSkillList> woochiSkills;

    public BaseSkill GetSkill(int id)
    {
        // id를 통해 skill을 찾아서 반환
        foreach (var skillEntry in woochiSkills)
        {
            foreach (var skillData in skillEntry.SkillDatas)
            {
                if (skillData.ID_Basic == id)
                {
                    return skillData.skill; // ID_Basic과 일치하는 skill 반환
                }
                else if (skillData.ID_Reinforced == id)
                {
                    return skillData.reinforcedSkill; // ID_Reinforced와 일치하는 reinforcedSkill 반환
                }
            }
        }
        
        Debug.LogWarning($"Skill not found for ID: {id}");
        return null;
    }
    
    
    #endregion WoochiSkill
    
    #region Charm
    [OneLineWithHeader, SerializeField] private List<Entry<BaseCharm>> charms;

    public BaseCharm GetCharm(int id)
    {
        if (id < 0)
        {
            Debug.Log("Library: ID out of range");
            return null;
        }

        return charms.FirstOrDefault(entry => entry.ID == id).value;
    }

    public int CharmCount => charms.Count;
    #endregion

    /// <summary>
    /// 구글 스프레드 시트의 데이터를 가져와서 스크립터블 초기화 -> 딱 한번만
    /// </summary>
    public void Initialize()
    {
        foreach (var character in characters)
        {
            if (character.value == null) continue;

            BaseCharacter baseCharacter = character.value.GetComponent<BaseCharacter>();
            baseCharacter.InitializeStatSO();
        }

        foreach (var abnormal in abnormals)
        {
            abnormal.value.Initialize();
        }
    }
}

[System.Serializable]
public class Entry<T>
{
    public int ID;
    public T value;
}

[System.Serializable]
public class RewardList
{
    public RareType rarity;
    public List<Reward> rewards;
}

[System.Serializable]
public class WoochiSkillList
{
    public SkillElement element;
    public List<SkillData> SkillDatas;
}


[System.Serializable]
public class SkillData
{
    public int ID_Basic;
    public BaseSkill skill;
    public int ID_Reinforced;
    public BaseSkill reinforcedSkill;
}