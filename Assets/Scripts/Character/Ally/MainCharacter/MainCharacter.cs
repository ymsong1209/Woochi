using System;
using UnityEngine;

public class MainCharacter : BaseCharacter
{
    public Action OnSorceryPointsChanged;
    
    [SerializeField] private int maxSorceryPoints = 200;
    [SerializeField] private int sorceryPoints = 200;
    [SerializeField] private float sorceryRecoveryPoints = .35f;

    [SerializeField] private MC_SorceryRecovery recoverySkill;
    [SerializeField] private MC_Summon summonSkill;
    [SerializeField] private MC_Charm charmSkill;
    [SerializeField] private MC_ChangeLocation changeLocation;
    
    private BaseSkill[] mainCharacterSkills = new BaseSkill[(int)SkillElement.END];
    public override void Initialize()
    {
        base.Initialize();

        maxSorceryPoints = DataCloud.playerData.maxSorceryPoints;
        sorceryPoints = DataCloud.playerData.sorceryPoints;

        recoverySkill.SkillOwner = this;
        summonSkill.SkillOwner = this;
        charmSkill.SkillOwner = this;
        changeLocation.SkillOwner = this;

        summonSkill.SkillRadius = new bool[] {true, true, true, true, false, false, false, false};
        changeLocation.SkillRadius = new bool[] {true, true, true, true, false, false, false, false};
    }

    public override void InitializeHealth()
    {
        base.InitializeHealth();
        
        SorceryPoints = maxSorceryPoints;
    }

    public override void InitializeSkill()
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
    }

    public override bool CheckUsableSkill()
    {
        return true;
    }
    
    public override void SaveStat()
    {
        base.SaveStat();

        DataCloud.playerData.maxSorceryPoints = maxSorceryPoints;
        DataCloud.playerData.sorceryPoints = sorceryPoints;
    }

    private void DeleteCharacterSkill()
    {
        for(int i = 0; i < mainCharacterSkills.Length; i++)
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

    public override void SetDead()
    {
        GameManager.GetInstance.ResetGame();

        HelperUtilities.MoveScene(SceneType.Title);
    }
    
    public void UpdateSorceryPoints(int value, bool isAdd)
    {
        if(isAdd == false)
            value *= -1;
        
        sorceryPoints = Mathf.Clamp(sorceryPoints + value, 0, maxSorceryPoints);
    }
    
    public void UpdateSorceryPoints(float percent, bool isAdd)
    {
        int value = Mathf.CeilToInt(maxSorceryPoints * percent);
        UpdateSorceryPoints(value, isAdd);
    }

    protected override void LevelUp()
    {
        base.LevelUp();
        SorceryPoints = maxSorceryPoints;
    }

    public BaseSkill[] MainCharacterSkills => mainCharacterSkills;
    
    public int SorceryPoints
    {
        get => sorceryPoints;
        set
        {
            sorceryPoints = value;
            OnSorceryPointsChanged?.Invoke();
        }
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
    
