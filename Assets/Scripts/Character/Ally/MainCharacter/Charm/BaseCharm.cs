using System.Collections;
using System.Collections.Generic;
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

   
   #region Getter Setter

   public int Turns => turns;
   public string CharmName => charmName;
   public bool[] CharmRadius => charmRadius;
   public CharmType CharmType => charmType;
   public CharmTargetType CharmTargetType => charmTargetType;
   public Sprite CharmIcon => charmIcon;

   #endregion
}
