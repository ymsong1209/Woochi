using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : BaseCharacter
{
    [SerializeField] private int maxSorceryPoints = 200;
    [SerializeField] private int sorceryPoints = 200;
    
    public int SorceryPoints
    {
        get
        { return sorceryPoints;
        }
    }
    
    public int MaxSorceryPoints
    {
        get
        {
            return maxSorceryPoints;
        }
    }
}
