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

    public virtual void AddBuff(BaseCharacter _buffOwner)
    {
        buffOwner = _buffOwner;
    }

    /// <summary>
    /// ���尡 ���۵ɶ� ����Ǵ� ����
    /// </summary>
    public virtual void ApplyRoundStartBuff()
    {

    }

    /// <summary>
    /// �ڽ��� ���ʰ� ������ ����
    /// </summary>
    public virtual void ApplyBuffWhenMyTurn()
    {
        --buffDurationTurns;
    }

    /// <summary>
    /// ���尡 ������ ����Ǵ� ����
    /// </summary>
    public virtual void ApplyRoundEndBuff()
    {

    }

    public virtual void RemoveBuff()
    {
        if(buffOwner == null)
        {
            Debug.LogError("No BuffOwner"); return;
        }


    }

    #region Getter Setter
    public int BuffDurationTurns
    {
        get { return buffDurationTurns; }
        set { buffDurationTurns = value; }
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

