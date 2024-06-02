using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSkill : BaseSkill
{
    [SerializeField] protected int requiredSorceryPoints;
    
    public int RequiredSorceryPoints
    {
        get
        {
            return requiredSorceryPoints;
        }
    }
}
