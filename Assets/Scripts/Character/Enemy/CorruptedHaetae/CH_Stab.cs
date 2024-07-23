using UnityEngine;

public class CH_Stab : BaseSkill
{
    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        base.ActivateSkill(_Opponent);
        float randomValue = Random.Range(0, 100);
        //40%의 확률로 적을 강제 이동
        if (randomValue < 40)
        {
            BattleManager.GetInstance.MoveCharacter(SkillResult.Opponent, -1);
        }
    }
    
    protected override void ApplyStat(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.Health;
        //최소, 최대 대미지 사이의 수치를 고름
        
        float RandomStat = Random.Range(SkillOwner.Stat.minStat, SkillOwner.Stat.maxStat);
        //피해량 계수를 곱함
        RandomStat *= (Multiplier / 100);
        if (_isCrit) RandomStat = RandomStat * 2;
        //부정한 찌르기는 방어력을 무시하고 대미지를 준다.
        
        opponentHealth.ApplyDamage((int)Mathf.Round(RandomStat), _isCrit);
        _opponent.CheckDeadAndPlayAnim();
    }
}
