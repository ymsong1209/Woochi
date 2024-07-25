using UnityEngine;

public class CH_Stab : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        //40%의 확률로 기절 디버프 부여
        GameObject stunDebuffPrefab = BuffPrefabList[0];
        GameObject stunDebuffGameObject = Instantiate(stunDebuffPrefab, transform);
        StunDeBuff stunDebuff = stunDebuffGameObject.GetComponent<StunDeBuff>();
        stunDebuff.BuffDurationTurns = 3;
        stunDebuff.ChanceToApplyBuff = 40;
        instantiatedBuffList.Add(stunDebuffGameObject);
        
        base.ActivateSkill(_Opponent);
        
        float randomValue = Random.Range(0, 100);
        //40%의 확률로 적을 강제 이동
        if (randomValue < 40)
        {
            BattleManager.GetInstance.MoveCharacter(SkillResult.Opponent, -1);
        }
        

    }
    protected override float CalculateDamage(BaseCharacter receiver, bool isCrit)
    {
        Health opponentHealth = _opponent.Health;
        //최소, 최대 대미지 사이의 수치를 고름
        
        float RandomStat = Random.Range(SkillOwner.Stat.minStat, SkillOwner.Stat.maxStat);
        RandomStat *= (Multiplier / 100);
        //부정한 찌르기는 방어력을 무시하고 대미지를 준다.
        if (isCrit) RandomStat = RandomStat * 2;
        return RandomStat;
    }
}
