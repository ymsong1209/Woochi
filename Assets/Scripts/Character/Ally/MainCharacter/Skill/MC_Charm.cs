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
   
   private void PlayAnimation(BaseCharacter opponent)
   {
       MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
       if (!mainCharacter) return;
       //우치는 항상 애니메이션 재생
       mainCharacter.onPlayAnimation?.Invoke(AnimationType.Skill1);

       if (mainCharacter != opponent)
       {
           PlayOpponentAnimation(opponent);
       }
      
   }

   protected virtual void PlayOpponentAnimation(BaseCharacter opponent)
   {
       if (charm.CharmTargetType == CharmTargetType.Singular ||
           charm.CharmTargetType == CharmTargetType.SingularWithSelf)
       {
           if ( charm.CharmType == CharmType.Buff || charm.CharmType == CharmType.CleanseSingleDebuff)
           {
               //TODO : 상대방 버프 애니메이션 재생
               opponent?.onPlayAnimation?.Invoke(AnimationType.Skill0);
           }
           else if (charm.CharmType == CharmType.DeBuff)
           {
               opponent?.onPlayAnimation?.Invoke(AnimationType.Damaged);
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
                   if (charm.CharmType == CharmType.Buff)
                   {
                       //TODO : 상대방 버프 애니메이션 재생
                       receiver.onPlayAnimation?.Invoke(AnimationType.Skill0);
                   }
                   else if (charm.CharmType == CharmType.DeBuff|| charm.CharmType == CharmType.CleanseSingleDebuff)
                   { 
                       receiver.onPlayAnimation?.Invoke(AnimationType.Damaged);
                   }
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
