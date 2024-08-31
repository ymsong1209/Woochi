using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : BaseCharacter
{
    [SerializeField] private int maxSorceryPoints = 200;
    [SerializeField] private int sorceryPoints = 200;
    [SerializeField] private float sorceryRecoveryPoints = 35f;

    [SerializeField] private MC_SorceryRecovery recoverySkill;
    [SerializeField] private MC_Summon summonSkill;
    [SerializeField] private MC_Charm charmSkill;
    [SerializeField] private MC_ChangeLocation changeLocation;
    
    private BaseSkill[] mainCharacterSkills = new BaseSkill[(int)SkillElement.END];
    public override void Initialize()
    {
        base.Initialize();
        recoverySkill.SkillOwner = this;
        summonSkill.SkillOwner = this;
        charmSkill.SkillOwner = this;
        changeLocation.SkillOwner = this;

        summonSkill.SkillRadius = new bool[] {true, true, true, true, false, false, false, false};
        changeLocation.SkillRadius = new bool[] {true, true, true, true, false, false, false, false};

    }

    protected override void InitializeSkill()
    {
        DeleteCharacterSkill();
        
        for (int i = 0; i < 5; ++i)
        {
            int skillId = DataCloud.playerData.currentskillIDs[i];
            if (skillId == 0) continue;
            
            BaseSkill skill = GameManager.GetInstance.Library.GetSkill(skillId);
            BaseSkill newSkill = Instantiate(skill, this.transform);
            newSkill.Initialize(this);
            mainCharacterSkills[(int)skill.SkillSO.SkillElement] = newSkill;
        }
        
        // //activeSkills의 size만큼 CharacterStat의 skill을 앞에서부터 가져와서 세팅한다.
        // for(int i = 0; i < activeSkillCheckBox.Count; ++i)
        // {
        //     //먼저 동일한 skillElement가 있는지 확인한다.
        //     BaseSkill statskill = characterStat.Skills[i];
        //     if(mainCharacterSkills[(int)statskill.SkillSO.SkillElement]){
        //        Debug.LogError("동일한 SkillElement가 존재합니다.");
        //        continue;
        //     }
        //     
        //     //새로운 스킬 생성후, 초기화
        //     BaseSkill newSkill = Instantiate(characterStat.Skills[i], this.transform);
        //     newSkill.Initialize(this);
        //     if (activeSkillCheckBox[i])
        //     {
        //         mainCharacterSkills[(int)statskill.SkillSO.SkillElement] = newSkill;
        //     }
        // }
    }

    private void DeleteCharacterSkill()
    {
        for(int i = 0;i<mainCharacterSkills.Length;i++)
        {
            DeleteCharacterSkillFromIndex(i);
        }
    }
    
    private void DeleteCharacterSkillFromIndex(int index)
    {
        BaseSkill skill = mainCharacterSkills[index];
        if (skill)
        {
            Destroy(skill.gameObject);
        }
        mainCharacterSkills[index] = null;
    }
    
    
    public BaseSkill[] MainCharacterSkills => mainCharacterSkills;
    
    public int SorceryPoints
    {
        get => sorceryPoints;
        set => sorceryPoints = value;
    }

    public int MaxSorceryPoints
    {
        get => maxSorceryPoints;
        set => maxSorceryPoints = value;
    }

    public float SorceryRecoveryPoints
    {
        get => sorceryRecoveryPoints;
        set => sorceryRecoveryPoints = value;
    }

    public MC_SorceryRecovery SorceryRecoverySkill => recoverySkill;
    public MC_Charm CharmSkill => charmSkill;

    public MC_Summon SummonSkill => summonSkill;

    public MC_ChangeLocation ChangeLocation => changeLocation;
}
    
