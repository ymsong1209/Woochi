using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MC_Charm : BaseSkill
{
   [SerializeField] private BaseCharm charm;


   public override void ActivateSkill(BaseCharacter opponent)
   {
       if (opponent == null) return;
       ActivateCharm(opponent);
       PlayAnimation(opponent);
       RemoveCharm();
   }

   private void ActivateCharm(BaseCharacter opponent)
   {
       BaseCharacter mainCharacter = BattleManager.GetInstance.currentCharacter;

       if (!charm)
       {
           Debug.LogError("No Charm Selected");
           return;
       }

       if (!opponent)
       {
           Debug.LogError("No Opponent");
           return;
       }
       SkillRadius = charm.CharmRadius;
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
           ApplyMultiple();
       }
   }

   protected override void ApplyMultiple()
   {
       Formation allies = BattleManager.GetInstance.Allies;
       Formation enemies = BattleManager.GetInstance.Enemies;
        
       List<BaseCharacter> receivers = new List<BaseCharacter>();
       for (int i = 0; i < charm.CharmRadius.Length; ++i)
       {
           if (i<4 && charm.CharmRadius[i])
           {
               BaseCharacter ally = allies.formation[i];
               if (!ally) continue;
                
               //아군의 Size가 2인 경우
               if (ally.Size == 2)
               {
                   // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                   if (!receivers.Any(e => e.gameObject == ally.gameObject))
                   {
                       receivers.Add(ally);
                   }
               }
               else
               {
                   // Size가 1인 Ally은 그냥 추가
                   receivers.Add(ally);
               }
           }
           else if (i is >= 4 and < 8 && charm.CharmRadius[i])
           {
               BaseCharacter enemy = enemies.formation[i - 4];
               if(!enemy) continue;

               //적의 Size가 2인 경우
               if(enemy.Size == 2)
               {
                   // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                   if (!receivers.Any(e => e.gameObject == enemy.gameObject))
                   {
                       receivers.Add(enemy);
                   }
               }
               else
               {
                   // Size가 1인 적은 그냥 추가
                   receivers.Add(enemy);
               }

           }
       }
       foreach (BaseCharacter opponent in receivers)
       {
           charm.Activate(opponent);
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
               //TODO : 상대방 버프 받는 애니메이션 재생
               opponent?.onPlayAnimation?.Invoke(AnimationType.Idle);
           }
           else if (charm.CharmType == CharmType.DeBuff)
           {
               opponent?.onPlayAnimation?.Invoke(AnimationType.Damaged);
           }
       }
       else if (charm.CharmTargetType == CharmTargetType.Multiple)
       {
           PlayMultipleOpponentAnimation();
       }
   }

   private void PlayMultipleOpponentAnimation()
   {
       Formation allies = BattleManager.GetInstance.Allies;
       Formation enemies = BattleManager.GetInstance.Enemies;
        
       List<BaseCharacter> receivers = new List<BaseCharacter>();
       for (int i = 0; i < charm.CharmRadius.Length; ++i)
       {
           if (i<4 && charm.CharmRadius[i])
           {
               BaseCharacter ally = allies.formation[i];
               if (!ally) continue;
                
               //아군의 Size가 2인 경우
               if (ally.Size == 2)
               {
                   // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                   if (!receivers.Any(e => e.gameObject == ally.gameObject))
                   {
                       receivers.Add(ally);
                   }
               }
               else
               {
                   // Size가 1인 Ally은 그냥 추가
                   receivers.Add(ally);
               }
           }
           else if (i is >= 4 and < 8 && charm.CharmRadius[i])
           {
               BaseCharacter enemy = enemies.formation[i - 4];
               if(!enemy) continue;

               //적의 Size가 2인 경우
               if(enemy.Size == 2)
               {
                   // 이미 Receivers 리스트에 동일한 GameObject를 참조하는 BaseCharacter가 없는 경우에만 추가
                   if (!receivers.Any(e => e.gameObject == enemy.gameObject))
                   {
                       receivers.Add(enemy);
                   }
               }
               else
               {
                   // Size가 1인 적은 그냥 추가
                   receivers.Add(enemy);
               }

           }
       }
       foreach (BaseCharacter opponent in receivers)
       {
           if (charm.CharmType == CharmType.Buff || charm.CharmType == CharmType.CleanseSingleDebuff)
           {
               //TODO : 상대방 버프 받는 애니메이션 재생
               opponent.onPlayAnimation?.Invoke(AnimationType.Idle);
           }
           else if (charm.CharmType == CharmType.DeBuff)
           { 
               opponent.onPlayAnimation?.Invoke(AnimationType.Damaged);
           }
       }
   }

   private void RemoveCharm()
   {
        var list = DataCloud.playerData.battleData.charms;

        for(int i = 0; i < list.Count; ++i)
        {
            if (list[i] == charm.ID)
            {
                list.RemoveAt(i);
                break;
            }
        }
   }

   public void SetSkillForCharm(BaseCharm selectedCharm)
   {
       if (!selectedCharm) return;
       this.charm = selectedCharm;
       SkillRadius = charm.CharmRadius;
   }
}
