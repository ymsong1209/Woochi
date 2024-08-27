using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseCharm : MonoBehaviour
{
   [SerializeField] private int turns;
   [SerializeField] private string charmName;
   ///0~4 : 아군 5~8 : 적군
   [SerializeField] private bool[] charmRadius = new bool[8];

   [SerializeField] private CharmType charmType;
   [SerializeField] private CharmTargetType charmTargetType;
   [SerializeField] private Sprite charmIcon;
   
   public virtual void Activate(BaseCharacter opponent)
   {
     
   }

   public virtual void SetCharmDescription(TextMeshProUGUI text)
    {
        string description = charmName + "\n";

        if (charmType == CharmType.CleanseSingleDebuff)
        {
            description += "자신 및 아군에게 걸린 디버프 중 하나를 랜덤하게 제거";
            text.text = description;
            return;
        }

        description += turns + "턴 동안 ";
        if (charmType == CharmType.Buff)
        {
            description += "아군 ";
        }
        else if (charmType == CharmType.DeBuff)
        {
            description += "적군 ";
        }

        if (charmTargetType == CharmTargetType.Singular)
        {
            description += GetCharmRadiusDescription(false);
        }
        else if (charmTargetType == CharmTargetType.Multiple)
        {
            description += GetCharmRadiusDescription(false);
        }
        else if (charmTargetType == CharmTargetType.SingularWithSelf)
        {
            description += GetCharmRadiusDescription(true);
        }
        // else if (charmTargetType == CharmTargetType.MultipleWithSelf)
        // {
        //     description += GetCharmRadiusDescription(true);
        // }

        text.text = description;
    }

    private string GetCharmRadiusDescription(bool includeSelf)
    {
        List<int> affectedRows = new List<int>();
        for (int i = 0; i < charmRadius.Length; i++)
        {
            if (charmRadius[i])
            {
                if (i <= 3)
                {
                    affectedRows.Add(i + 1); // 아군은 1부터 4까지
                }
                else
                {
                    affectedRows.Add(i - 3); // 적군은 1부터 4까지
                }
            }
        }

        string radiusDescription = "";
        if (affectedRows.Count > 0)
        {
            radiusDescription = string.Join(", ", affectedRows) + "열";
            if (includeSelf)
            {
                radiusDescription += " 및 자신의\n";
            }
        }
        return radiusDescription;
    }
   
   
   
   #region Getter Setter

   public int Turns => turns;
   public string CharmName => charmName;
   public bool[] CharmRadius => charmRadius;
   public CharmType CharmType => charmType;
   public CharmTargetType CharmTargetType => charmTargetType;
   public Sprite CharmIcon => charmIcon;

   #endregion
}
