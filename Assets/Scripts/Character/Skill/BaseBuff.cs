using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BaseBuff : MonoBehaviour
{

    [SerializeField] protected BaseCharacter buffOwner;
    /// <summary>
    /// buffDurationTurns�� -1�̸� �������� ����
    /// </summary>
    [SerializeField] protected int buffDurationTurns;
    [SerializeField] protected BuffType buffType;

    /// <summary>
    /// ������ �߰�
    /// </summary>
    public virtual void AddBuff(BaseCharacter _buffOwner)
    {
        buffOwner = _buffOwner;
    }

    /// <summary>
    /// ������ ���۵ɶ� ����Ǵ� ���� ȿ��
    /// ��� �� ĳ���Ͱ� ��������� true ��ȯ
    /// <returns></returns>
    public virtual bool ApplyBattleStartBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ���尡 ���۵ɶ� ����Ǵ� ����
    /// ���� ȿ�� ��� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool ApplyRoundStartBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// �ڽ��� ���ʰ� ������ ����
    /// ���� ȿ�� ��� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool ApplyTurnStartBuff()
    {
        --buffDurationTurns;
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ���尡 ������ ����Ǵ� ����
    /// ���� ȿ�� ��� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool ApplyRoundEndBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ������ ���� �� ����Ǵ� ���� ȿ��, default�� ������ ���ŵǰ� ��
    /// ��� �� ĳ���� ��������� true ��ȯ
    /// </summary>
    public virtual bool ApplyBattleEndBuff()
    {
        RemoveBuff();
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// ���� ����
    /// ���� ���� �� ĳ���Ͱ� ��������� true��ȯ
    /// </summary>
    public virtual bool RemoveBuff()
    {
        if (buffOwner.CheckDead() == false) return true;
        return false;
    }

    /// <summary>
    /// _bufftype�� ������ ����� �� �ִ��� Ȯ��
    /// </summary>
    public virtual bool ValidateApplyBuff(BuffType _bufftype)
    {
        return true;
    }

    #region Getter Setter
    public int BuffDurationTurns
    {
        get { return buffDurationTurns; }
        set { buffDurationTurns = value; }
    }
    public BuffType BuffType
    {
        get { return buffType; }
        set { buffType = value; }
    }
    #endregion Getter Setter

    #region Validation
    private void OnValidate()
    {
        if(buffDurationTurns == 0)
        {
            Debug.Log(nameof(buffDurationTurns) + " is not over 1 in object " + this.name.ToString());
        }
        
    }
    #endregion
}

