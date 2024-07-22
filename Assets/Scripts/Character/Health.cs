using System;
using UnityEngine;
using UnityEngine.Events;


[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private BaseCharacter owner;
    [SerializeField] private int maxHealth;
    [SerializeField] private int curHealth;
    [SerializeField] private int shield;


    private void Awake()
    {
        owner = GetComponent<BaseCharacter>();
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
        }
        owner.onAttacked?.Invoke(AttackResult.Heal, _healamount, false);
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



    #region Getter Setter

    public int MaxHealth
    {
        get { return maxHealth; }
        set { 
            maxHealth = value;
        }
    }

    public int CurHealth
    {
        get { return curHealth; }
        set 
        { 
            curHealth = value; 
            owner.Stat.curHealth = value;
            owner.onHealthChanged?.Invoke();
        }
    }

    #endregion

    #region Validation
    private void OnValidate()
    {
        if(maxHealth < curHealth)
        {
            Debug.Log("Maxhealth is below than Curhealth in " + this.gameObject.name);
        }
    }
    #endregion Validation
}