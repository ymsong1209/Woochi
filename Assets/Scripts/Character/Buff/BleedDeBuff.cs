using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedDeBuff : BaseBuff
{
    //�� ���� �����Ҷ� �� �ۼ�Ʈ��ŭ �Ǹ� ��������
    [SerializeField,ReadOnly] private int bleedPercent = 0;
    //���� ������� �ɶ� ��%��ŭ ���������� ���� ������
    [SerializeField] protected int bleedApply;
    //bleed���� �����
    public override bool ApplyTurnStartBuff()
    {
        //��üü�¿��� bleedApply%��ŭ �Ǹ� ��´�.
        float bleedDamage = buffOwner.Health.MaxHealth * bleedApply / 100f;
        buffOwner.Health.ApplyDamage(bleedDamage);

        --buffDurationTurns;
        //TODO : BleedApply������ �ٲ���� ����.

        Debug.Log(buffOwner.name + "is Bleeding. Bleed leftover turn : " + buffDurationTurns.ToString());

        //checkdead�� ĳ���Ͱ� �׾������ true ��ȯ
        //ApplyTurnStartBuff�� ���� ���� �� ĳ���Ͱ� ������� true ��ȯ
        return (!buffOwner.CheckDead());
    }

    public override void StackBuff()
    {
        //TODO : ���� ���� ���� ��� ��� ����?
        Debug.Log("BleedBuff Stacked.");
        //base.StackBuff();
    }
}
