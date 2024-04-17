using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiger_Rip : BaseSkill
{
    public override void ApplySkill(BaseCharacter _Opponent)
    {
        //�Ʊ� ��ȣ ��ų������ ��ȣ �� �� ����
        //���������� �����ؾ��ϴ� �� ����
        BaseCharacter opponent = CheckOpponentValid(_Opponent);

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
            Debug.Log(_Opponent.name.ToString() + "Evaded skill" + Name);
            return;
        }

        //ġ��Ÿ�� ��� �ٷ� ���� ����
        if (CheckCrit())
        {
            Debug.Log("Crit Skill on " + Name + "to " + _Opponent.name.ToString());
            ApplyDamage(opponent, true);

            //���� ����
            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (ApplybuffGameobject == null) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                if (BufftoApply == null) continue;
                //���� buff/debuff�� ��%�� Ȯ���� �ɸ����� �Ǵ�.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //ġ��Ÿ�� ���� ������ä ��ų ����
                ApplyBuff(opponent, BufftoApply);
            }
        }
        else
        {
            Debug.Log("Non Crit Skill on " + Name + "to " + _Opponent.name.ToString());
            ApplyDamage(opponent, false);

            foreach (GameObject ApplybuffGameobject in bufflist)
            {
                if (ApplybuffGameobject == null) continue;
                BaseBuff BufftoApply = ApplybuffGameobject.GetComponent<BaseBuff>();
                //���� buff/debuff�� ��%�� Ȯ���� �ɸ����� �Ǵ�.
                if (CheckApplyBuff(BufftoApply) == false) continue;
                //���� ���� ��ġ �Ǵ�.
                if (CheckResist(opponent) == false)
                {
                    ApplyBuff(opponent, BufftoApply);
                }

            }
        }

    }

    private void ApplyDamage(BaseCharacter _opponent, bool _isCrit)
    {
        Health opponentHealth = _opponent.gameObject.GetComponent<Health>();
        //�ּ�, �ִ� ����� ������ ��ġ�� ��

        float RandomStat = Random.Range(SkillOwner.MinStat, SkillOwner.MaxStat);
        //���ط� ����� ����
        RandomStat *= (Multiplier / 100);

        //��� ������ ��
        RandomStat = RandomStat * (100 - _opponent.Defense) / 100;

        //������ ���� ������ �پ������� 1.5���� �����
        bool hasBleed = false;
        foreach(BaseBuff buff in _opponent.activeBuffs)
        {
            if(buff.BuffType == BuffType.Bleed)
            {
                hasBleed = true;
            }
        }
        if (hasBleed) RandomStat = RandomStat * 1.5f;

        //ġ��Ÿ�� ��� ����������� 2��
        if (_isCrit) RandomStat = RandomStat * 2;

        opponentHealth.ApplyDamage((int)Mathf.Round(RandomStat));
    }
}
