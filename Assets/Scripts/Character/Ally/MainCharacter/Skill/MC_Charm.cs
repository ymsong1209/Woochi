using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_Charm : BaseSkill
{
   [SerializeField] private BaseCharm charm;


   public override void ActivateSkill(BaseCharacter opponent)
   {
       ActivateCharm(opponent);
       PlayAnimation(opponent);
       GameManager.GetInstance.RemoveCharm(charm);
   }

   private void PlayAnimation(BaseCharacter opponent)
   {
       MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
       if (!mainCharacter) return;
       //우치는 항상 애니메이션 재생
       mainCharacter.onPlayAnimation?.Invoke(AnimationType.Skill0);

       if (charm.CharmType == CharmType.Buff)
       {
           if (charm.CharmTargetType == CharmTargetType.Singular ||
               charm.CharmTargetType == CharmTargetType.SingularWithSelf)
           {
               if (opponent != mainCharacter)
               {
                   opponent?.onPlayAnimation?.Invoke(AnimationType.Skill0);
               }
           }
           else if (charm.CharmTargetType == CharmTargetType.Multiple ||
                    charm.CharmTargetType == CharmTargetType.MultipleWithSelf)
           {
               for (int i = 0; i < charm.CharmRadius.Length; ++i)
               {
                   if (charm.CharmRadius[i])
                   {
                       BaseCharacter receiver = BattleManager.GetInstance.GetCharacterFromIndex(i);
                       if (receiver != mainCharacter) receiver.onPlayAnimation?.Invoke(AnimationType.Skill0);
                   }
               }
           }
       }
   }

   private void ActivateCharm(BaseCharacter opponent)
   {
       BaseCharacter mainCharacter = BattleManager.GetInstance.currentCharacter;
       if (charm.CharmTargetType == CharmTargetType.Singular)
       {
           charm.Activate(opponent);
       }
       else if (charm.CharmTargetType == CharmTargetType.SingularWithSelf)
       {
           charm.Activate(opponent);
           if (opponent != mainCharacter)
           {
               charm.Activate(mainCharacter);
           }
       }
       else if (charm.CharmTargetType == CharmTargetType.Multiple)
       {
           for (int i = 0; i < charm.CharmRadius.Length; ++i)
           {
               if (charm.CharmRadius[i])
               {
                   BaseCharacter receiver = BattleManager.GetInstance.GetCharacterFromIndex(i);
                   charm.Activate(receiver);
               }
           }
       }
       else if (charm.CharmTargetType == CharmTargetType.MultipleWithSelf)
       {
           charm.Activate(mainCharacter);
           for (int i = 0; i < charm.CharmRadius.Length; ++i)
           {
               if (charm.CharmRadius[i])
               {
                   BaseCharacter receiver = BattleManager.GetInstance.GetCharacterFromIndex(i);
                   if(receiver!= mainCharacter) charm.Activate(receiver);
               }
           }
       }
   }

   public void SetSkillForCharm()
   {
       if (!charm) return;
       SkillRadius = charm.CharmRadius;
   }
   
   public BaseCharm Charm
   {
      get => charm;
      set => charm = value;
   }
}
