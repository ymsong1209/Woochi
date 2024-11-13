using OneLine;
using System.Collections.Generic;
using System.Linq;
using DataTable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows.WebCam;

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
    
    [OneLineWithHeader, SerializeField] private List<Entry<Strange>> stranges;

    public Strange GetStrange(int id)
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

    /// <summary>
    /// 우치의 경지(레벨)에 따라서 드롭 확률을 정하고, 스킬을 랜덤으로 반환하는 함수
    /// </summary>
    /// <returns>Library에 도술이 안 등록되어있으면, -1을 반환</returns>
    public int GetRandomSkillIDByRank()
    {
        int CurRank = BattleManager.GetInstance.Allies.GetWoochi().level.rank;
        WoochiSkillPropabilityData data = WoochiSkillPropabilityData.GetDictionary()[CurRank];
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
    
    /// <summary>
    /// 기본 스킬 ID를 받아서 강화된 스킬 ID를 반환하는 함수
    /// 강화된 스킬을 넣으면 자기 자신을 반환
    /// </summary>
    /// <param name="basicSkillID">기본 스킬 id를 입력</param>
    /// <returns>유효하지 않는 스킬을 넣으면 -1반환</returns>
    public int GetEnhancedSkillID(int basicSkillID)
    {
        foreach (var skillEntry in woochiSkills)
        {
            foreach (var skillData in skillEntry.SkillDatas)
            {
                if (skillData.ID_Basic == basicSkillID || skillData.ID_Reinforced == basicSkillID)
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
                MainCharacterSkillSO skillSO = skill.SkillSO as MainCharacterSkillSO;
                // rarity가 주어진 값과 일치하는 경우
                if (skillSO.Rarity == rarity)
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


    /// <summary>
    /// 신규로 뽑은 도술을 도술두루마리에 추가하는 함수
    /// </summary>
    /// <param name="baseskillid">인자로 일반 스킬 id만 들어와야함.</param>
    /// <returns>도술 두루마리에 신규로 추가할 수 있거나, 같은 기본 </returns>
    public SkillSetResult SetSkillOnScroll(int baseskillid)
    {
        SkillSetResult result = new SkillSetResult();
        result.skillID = baseskillid;
        int[,] totalskillIDs = DataCloud.playerData.totalSkillIDs;
        
        BaseSkill skill = GetSkill(baseskillid);
        SkillElement element = skill.SkillSO.SkillElement;
        result.skillName = skill.SkillSO.SkillName;
        
        int enhancedSkillID = GetEnhancedSkillID(baseskillid);
        BaseSkill enhancedSkill = GetSkill(enhancedSkillID);
        result.enhancedSkillName = enhancedSkill.SkillSO.SkillName;
        
        // 도술 두루마리의 같은 속성에 가서 같은 기본 도술,혹은 강화도술이 있는지 확인
        // 기본 도술이랑 강화 도술이 같이 있는 경우는 없음.

        bool hasSpace = false;
        int spaceIndex = -1;
        for (int i = 0; i < 5; ++i)
        {
            //스킬을 세팅할 자리가 있는 경우
            if(!hasSpace && totalskillIDs[(int)element-1,i] == 0)
            {
                hasSpace = true;
                spaceIndex = i;
            }
            //만약 동일한 일반 스킬이 있는 경우, 강화
            if(totalskillIDs[(int)element-1,i] == baseskillid)
            {
                totalskillIDs[(int)element-1,i] = enhancedSkillID;
                result.enhancedSkillID = enhancedSkillID;
                result.isEnhanced = true;
                result.isSuccess = true;
                DataCloud.playerData.totalSkillIDs = totalskillIDs;
                
                //강화시키려는 스킬을 우치가 장착하고 있는 경우, 강화된 스킬로 바꿔줌
                for (int j = 0; j < 5; ++j)
                {
                    if(DataCloud.playerData.currentskillIDs[j] == baseskillid)
                    {
                        DataCloud.playerData.currentskillIDs[j] = enhancedSkillID;
                    }
                }
                return result;
            }
            //강화된 버전의 스킬을 이미 가지고 있는 경우 실패
            else if(totalskillIDs[(int)element-1,i] == enhancedSkillID)
            {
                result.isSuccess = false;
                result.enhancedSkillID = enhancedSkillID;
                result.isSameSkill = true;
                DataCloud.playerData.totalSkillIDs = totalskillIDs;
                return result;
            }
        }
        //스킬을 세팅할 수 있으면, 가장 빠른 빈 자리에 넣어줌
        if (hasSpace && spaceIndex != -1)
        {
            totalskillIDs[(int)element-1,spaceIndex] = baseskillid;
            DataCloud.playerData.totalSkillIDs = totalskillIDs;
            result.isSuccess = true;
            result.isEnhanced = false;
        }
        //스킬을 세팅할 수 없는 경우 : 도술 두루마리가 꽉 찼을 때
        else
        {
            result.isSuccess = false;
            result.isScrollFull = true;
        }

        return result;
    }
    
    /// <summary>
    /// 인자로 넣은 skillid랑 동일한 속성을 가진 도술들의 ID를 반환하는 함수
    /// </summary>
    /// <returns></returns>
    int[] GetSkillIDsByElement(int skillID)
    {
        BaseSkill skill = GetSkill(skillID);
        SkillElement element = skill.SkillSO.SkillElement;
        int[,] totalskillIDs = DataCloud.playerData.totalSkillIDs;
        int[] skillIDs = new int[5];
        for (int i = 0; i < 5; ++i)
        {
            skillIDs[i] = totalskillIDs[(int)element-1,i];
        }

        return skillIDs;
    }
    
    void SetSkillIDByElement(SkillElement element, int[] skillIDs)
    {
        int[,] totalskillIDs = DataCloud.playerData.totalSkillIDs;
        for (int i = 0; i < 5; ++i)
        {
            totalskillIDs[(int)element-1,i] = skillIDs[i];
        }
        DataCloud.playerData.totalSkillIDs = totalskillIDs;
    }

    public void EquipSkill(int skillid, int position)
    {
        int[] currentskillIDs = DataCloud.playerData.currentskillIDs;
        currentskillIDs[position] = skillid;
        DataCloud.playerData.currentskillIDs = currentskillIDs;
    }
    
    #endregion WoochiSkill
    
    #region Charm
    [OneLineWithHeader, SerializeField] private List<Entry<BaseCharm>> charms;

    public BaseCharm GetCharm(int id)
    {
        if (id < 0)
        {
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
    [OneLineWithHeader] public List<SkillData> SkillDatas;
}


[System.Serializable]
public class SkillData
{
    public int ID_Basic;
    public BaseSkill skill;
    public int ID_Reinforced;
    public BaseSkill reinforcedSkill;
}


public struct SkillSetResult
{
    public int skillID;
    public int enhancedSkillID;
    public string skillName;
    public string enhancedSkillName;
        
    public bool isSuccess; //기본 도술 세팅 성공
        
    //---Success--//
    public bool isEnhanced; //도술을 넣어서 강화가 되었는지
        
    //--Fail--//
    public bool isScrollFull; //도술 두루마리가 꽉 찼는지
    public bool isSameSkill; //같은 도술이 이미 있는지
}