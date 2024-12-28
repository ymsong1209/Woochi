using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "T_", menuName = "Scriptable Objects/Character/MainCharacterSkill")]
public class MainCharacterSkillSO : SkillSO
{
    [SerializeField] private int requiredSorceryPoints;
    [SerializeField] private int rarity;

    public int RequiredSorceryPoints => requiredSorceryPoints;
    public int Rarity => rarity;
}
