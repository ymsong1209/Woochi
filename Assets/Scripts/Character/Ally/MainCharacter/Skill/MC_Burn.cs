using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MC_Burn : MainCharacterSkill
{
    public MC_Burn()
    {
        requiredSorceryPoints = 70;
    }
    public override void SetSkillDescription(TextMeshProUGUI text)
    {
        text.text = "불길\n" +
                    "도력 "+ requiredSorceryPoints+"을 소모하여\n" + 
                    "대상 전체에게 " + SkillSO.BaseMultiplier +"%의 피해";
    }
}
