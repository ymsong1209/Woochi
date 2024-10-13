using DataTable;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class Health
{
    [JsonIgnore] private BaseCharacter owner;
    [SerializeField, ReadOnly] private int maxHealth;
    [SerializeField, ReadOnly] private int curHealth;
    [SerializeField, ReadOnly] private int shield;

    // Resurrection
    [SerializeField] private int countToResurrect;

    public Health()
    {
        owner = null;
    }

    public Health(Health health)
    {
        maxHealth = health.maxHealth;
        curHealth = health.curHealth;
        shield = health.shield;
        countToResurrect = health.countToResurrect;
    }

    public Health(CharacterData characterData)
    {
        maxHealth = characterData.health;
        curHealth = maxHealth;
        shield = 0;
        countToResurrect = -1;
    }

    public void Initialize(BaseCharacter owner, Health health)
    {
        this.owner = owner;
        maxHealth = health.maxHealth;
        curHealth = health.curHealth;
        shield = health.shield;
        countToResurrect = health.countToResurrect;
    }

    /// <summary>
    /// 대미지를 받는 공식, _penetrate가 true일 경우에는 쉴드를 뚫는 관통형 대미지
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_penetrate">_penetrate가 true일 경우에는 쉴드를 뚫는 관통형 대미지 </param>
    public void ApplyDamage(int _damage, bool _isCrit = false, bool _penetrate = false)
    {
        //관통형 대미지인경우
        if (_penetrate == false)
        {
            CurHealth = Mathf.Clamp(CurHealth - _damage, 0, maxHealth);
            Debug.Log("Curhealth : " + curHealth);
        }
        //비관통형 대미지인경우 쉴드 먼저 까임
        else
        {
            shield -= _damage;
            if(shield < 0)
            {
                CurHealth += shield;
                shield = 0;
            }
        }

        owner.onAttacked?.Invoke(AttackResult.Normal, _damage, _isCrit);
    }

    /// <summary>
    /// 방어도를 0으로 초기화
    /// </summary>
    public void ResetShield()
    {
        shield = 0;
    }

    public void Heal(int _healamount, bool playAnimation = true)
    {
        CurHealth = Mathf.Clamp(CurHealth + _healamount, 0, maxHealth);
        if (playAnimation)
        {
            owner.onPlayAnimation?.Invoke(AnimationType.Heal);
            owner.onAttacked?.Invoke(AttackResult.Heal, _healamount, false);
        }
    }
    
    public void LevelUp()
    {
        maxHealth = owner.FinalStat.maxHealth;
        Heal(maxHealth, false);
    }

    public bool CheckHealthZero()
    {
        if (CurHealth <= 0)
        {
            //관통형 대미지로 체력이 0이 될수 있음
            shield = 0;
            return true;
        }
        else return false;
    }

    public void Resurrect()
    {
        CurHealth = maxHealth / 2;
        countToResurrect = -1;
    }

    #region Getter Setter

    public int MaxHealth
    {
        get { return maxHealth; }
        set { 
            maxHealth = value;

            if (owner != null)
                owner.onHealthChanged?.Invoke();
        }
    }

    public int CurHealth
    {
        get { return curHealth; }
        set 
        { 
            curHealth = value;

            if(owner != null)
                owner.onHealthChanged?.Invoke();
        }
    }

    public int TurnToResurrect
    {
        get { return countToResurrect; }
        set { countToResurrect = value; }
    }

    #endregion
}