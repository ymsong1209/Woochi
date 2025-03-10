using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseCharm : MonoBehaviour
{
    [Header("ID")]
    [SerializeField] public int ID;
    
    [Header("Info")]
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

    public void SetCharmEffect(TextMeshProUGUI text)
    {
        
        SetCharmDescription(text);

        string description = text.text;
        

        // if (charmTargetType == CharmTargetType.Singular || charmTargetType == CharmTargetType.Multiple)
        // {
        //     description += GetCharmRadiusDescription(false);
        // }
        // else if (charmTargetType == CharmTargetType.SingularWithSelf)
        // {
        //     description += GetCharmRadiusDescription(true);
        // }
        // else if (charmTargetType == CharmTargetType.MultipleWithSelf)
        // {
        //     description += GetCharmRadiusDescription(true);
        // }

        text.text = description;
    }

    protected virtual void SetCharmDescription(TextMeshProUGUI text)
    {
        text.text = charmName + "\n";
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
