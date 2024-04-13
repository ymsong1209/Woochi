using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Bite : BaseSkill
{
    public override void ApplySkill(BaseCharacter _Opponent)
    {
        //�Ʊ� ��ȣ ��ų������ ��ȣ �� �� ����
        //���������� �����ؾ��ϴ� �� ����
        BaseCharacter opponent = base.CheckOpponentValid(_Opponent);

        if (opponent == null)
        {
            Debug.LogError("opponent is null");
            return;
        }

        //���� üũ
        if (CheckAccuracy() == false)
        {
            Debug.Log("Accuracy Failed on" + _Opponent.name.ToString());
            return;
        }
        //ȸ�� üũ
        if (CheckEvasion(opponent) == false)
        {
            Debug.Log(_Opponent.name.ToString() + "Evaded skill" + base.Name);
            return;
        }

        //ġ��Ÿ�� ���
        if (CheckCrit())
        {
            Debug.Log("Crit Skill on " + base.Name + "to " + _Opponent.name.ToString());
            ApplyTotalDamage(opponent, true);

        }
        else
        {
            Debug.Log("Non Crit Skill on " + base.Name + "to " + _Opponent.name.ToString());
            ApplyTotalDamage(opponent, false);
        }
    }

    private void ApplyTotalDamage(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.gameObject.GetComponent<Health>();
        //�ּ�, �ִ� ����� ������ ��ġ�� ��

        float RandomStat = Random.Range(SkillOwner.MinStat, SkillOwner.MaxStat);
        //���ط� ����� ����
        RandomStat *= (Multiplier / 100);

        //��� ������ �� base ������ ����
        RandomStat = RandomStat * (100 - _opponent.Defense) / 100;
        if (_isCrit) RandomStat = RandomStat * 2;

        // ������� ���ظ� ������ ���� ���� ü���� 20% ��ŭ �߰� ���ظ� ��
        RandomStat +=  (opponentHealth.MaxHealth - opponentHealth.CurHealth) * 0.2f;

        //������ �������� ������� ��
        opponentHealth.ApplyDamage((int)Mathf.Round(RandomStat));

        //ȣ���̴� �� ������ 30%��ŭ ȸ����.
        int healamount = (int)Mathf.Round(RandomStat * 0.3f);
        SkillOwner.Health.Heal(healamount);

        // ���ظ� �Ծ��� �� �ִϸ��̼� ���
        _opponent.PlayAnimation(AnimationType.Damaged);
    }
}
