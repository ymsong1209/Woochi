public class MC_Summon : BaseSkill
{
    public BaseCharacter willSummon;    // ��ȯ�� ĳ����
    public bool isSummon = false;       // ��ȯ�� ������ ��ȯ ������ ������

    public override void ActivateSkill(BaseCharacter _Opponent)
    {
        // ��ȯ, ��ȯ ���� �� Ư���� �ִϸ��̼� ������ ���⼭ ó���� ��
        if(isSummon)
        {
            BattleManager.GetInstance.Summon(willSummon, _Opponent);
            willSummon.onPlayAnimation?.Invoke(AnimationType.Idle);
        }
        else
        {

        }


        SkillOwner.onPlayAnimation?.Invoke(AnimationType.Skill2);
    }
}
