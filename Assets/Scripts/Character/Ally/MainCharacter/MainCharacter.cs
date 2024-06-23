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
    
