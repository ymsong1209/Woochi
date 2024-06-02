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

    public int SorceryPoints
    {
        get { return sorceryPoints; }
        set { sorceryPoints = value; }
    }

    public int MaxSorceryPoints
    {
        get { return maxSorceryPoints; }
        set { maxSorceryPoints = value; }
    }

    public float SorceryRecoveryPoints
    {
        get { return sorceryRecoveryPoints; }
        set { sorceryRecoveryPoints = value; }
    }

    public MC_SorceryRecovery SorceryRecoverySkill
    {
        get { return recoverySkill; }
    }
}
    
