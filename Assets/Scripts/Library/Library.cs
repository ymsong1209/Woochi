using OneLine;
using System.Collections.Generic;
using System.Linq;
using DataTable;
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

    public int GetRandomSkillID()
    {
        int CurRank = BattleManager.GetInstance.Allies.GetWoochi().level.rank;
        WoochiSkillData data = WoochiSkillData.GetDictionary()[CurRank];
        // 확률에 따른 누적 값 계산
        float totalProbability = data.Lowest + data.Lower + data.Middle + data.Higher + data.Highest;
        // 랜덤 값을 생성 (0부터 totalProbability 사이)
        float randomValue = Random.Range(0f, totalProbability);

        // 확률에 따라 기본 스킬 ID 선택
        if (randomValue <= data.Lowest)
        {
            return GetRandomSkillIdByRarity(1);
        }
        else if (randomValue <= data.Lowest + data.Lower)
        {
            return GetRandomSkillIdByRarity(2);
        }
        else if (randomValue <= data.Lowest + data.Lower + data.Middle)
        {
            return GetRandomSkillIdByRarity(3);
        }
        else if (randomValue <= data.Lowest + data.Lower + data.Middle + data.Higher)
        {
            return GetRandomSkillIdByRarity(4);
        }
        else
        {
            return GetRandomSkillIdByRarity(5);
        }
    }
    
    public int GetEnhancedSkillID(int basicSkillID)
    {
        foreach (var skillEntry in woochiSkills)
        {
            foreach (var skillData in skillEntry.SkillDatas)
            {
                if (skillData.ID_Basic == basicSkillID)
                {
                    return skillData.ID_Reinforced;
                }
            }
        }
        
        Debug.LogWarning($"Enhanced Skill not found for ID: {basicSkillID}");
        return -1;
    }
    
    private int GetRandomSkillIdByRarity(int rarity)
    {
        List<int> skillList = new List<int>();

        foreach (var skillEntry in woochiSkills)
        {
            foreach (var skillData in skillEntry.SkillDatas)
            {
                MainCharacterSkill skill = skillData.skill as MainCharacterSkill;
                // rarity가 주어진 값과 일치하는 경우
                if (skill.Rarity == rarity)
                {
                    skillList.Add(skillData.ID_Basic);
                }
            }
        }

        // 스킬 리스트에서 랜덤으로 하나의 ID 반환
        if (skillList.Count > 0)
        {
            return skillList[Random.Range(0, skillList.Count)];
        }

        // 만약 해당 rarity에 해당하는 스킬이 없다면 -1 반환 (혹은 다른 기본값)
        return -1;
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