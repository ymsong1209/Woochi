using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "T_", menuName = "Scriptable Objects/Character/MainCharacterSkill")]
public class MainCharacterSkillSO : SkillSO
{
    [SerializeField] private int requiredSorceryPoints;
    [SerializeField] private int rarity;

    public int RequiredSorceryPoints => requiredSorceryPoints;
    public int Rarity => rarity;

    public override string GetSkillSound()
    {
        switch (SkillElement)
        {
            case SkillElement.Fire:
                return "Fire_Skill";
            case SkillElement.Water:
                return "Water_Skill";
            case SkillElement.Wood:
                return "Tree_Skill";
            case SkillElement.Metal:
                return "Metal_Skill";
            case SkillElement.Earth:
                return "Ground_Skill";
        }

        return string.Empty;
    }
}
