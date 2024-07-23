using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;


public class BaseBuff : MonoBehaviour
{

    private BaseCharacter caster; //버프를 건 사람
    [SerializeField] protected BaseCharacter buffOwner;
    /// <summary>
    /// 현재 버프가 몇턴 남았는지
    /// buffDurationTurns가 -1이면 영구지속 버프
    /// </summary>
    [SerializeField] protected int  buffDurationTurns;
    [SerializeField] protected BuffEffect buffEffect;
    [SerializeField] protected BuffType buffType;
    [SerializeField] protected int chanceToApplyBuff;
    [SerializeField] protected string buffName;
    

    /// <summary>
    /// 버프를 추가
    /// </summary>
    public virtual void AddBuff(BaseCharacter _caster, BaseCharacter receiver)
    {
        caster = _caster;
        buffOwner = receiver;
        buffOwner.activeBuffs.Add(this);
    }

    /// <summary>
    /// 전투가 시작될때 적용되는 버프 효과
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// <returns></returns>
    public virtual int ApplyBattleStartBuff()
    {
        return 0;
    }

    /// <summary>
    /// 라운드가 시작될때 적용되는 버프
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyRoundStartBuff()
    {
        return 0;
    }

    /// <summary>
    /// 자신의 차례가 됐을때 적용
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyTurnStartBuff()
    {
        if(buffDurationTurns > 0) --buffDurationTurns;
        return 0;
    }
    /// <summary>
    /// 자신의 턴이 끝났을때 적용
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyTurnEndBuff()
    {
        return 0;
    }

    /// <summary>
    /// 라운드가 끝날때 적용되는 버프
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyRoundEndBuff()
    {
        return 0;
    }

    /// <summary>
    /// 전투가 끝난 후 적용되는 버프 효과, default로 버프가 제거되게 함
    /// 버프를 적용하고 받은 대미지를 반환
    /// 사용 후 캐릭터가 죽었거나 턴이 스킵되었으면 -1반환
    /// </summary>
    public virtual int ApplyBattleEndBuff()
    {
        return 0;
    }

    /// <summary>
    /// 버프 제거
    /// </summary>
    public virtual void RemoveBuff()
    {
        //현재 parent에 가서 bufficon을 가져옴
        BuffIcon icon = transform.parent.GetComponent<BuffIcon>();
        if (icon)
        {
            this.transform.parent = null;
            if(icon.transform.childCount == 0) icon.DeActivate();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 중복해서 버프류를 쌓으려고 하는 경우
    /// </summary>
    public virtual void StackBuff(BaseBuff _buff)
    {
        buffDurationTurns += _buff.buffDurationTurns;
    }

    public virtual void SetBuffDescription(TextMeshProUGUI text)
    {
        text.text = "Buff Description\n";
        text.color = Color.magenta;
    }
    

    #region Getter Setter

    public BaseCharacter Caster => caster;
    public int BuffDurationTurns
    {
        get { return buffDurationTurns; }
        set { buffDurationTurns = value; }
    }
    public BuffEffect BuffEffect
    {
        get => buffEffect;
        set => buffEffect = value;
    }
    
    public BuffType BuffType
    {
        get => buffType;
        set => buffType = value;
    }
    
    public int ChanceToApplyBuff
    {
        get => chanceToApplyBuff;
        set => chanceToApplyBuff = value;
    }

    public string BuffName
    {
        get => buffName;
        set => buffName = value;
    }

    #endregion Getter Setter

    #region Validation
    private void OnValidate()
    {
        if (buffType == BuffType.Default)
        {
            Debug.Log(  this.name +  "의 buffEffect이 Default로 설정되어 있습니다. 설정을 바꿔주세요");
        }
        
    }
    #endregion
}

