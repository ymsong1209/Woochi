using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int curHealth;
    [SerializeField] private int shield;

    /// <summary>
    /// 대미지를 받는 공식, _penetrate가 true일 경우에는 쉴드를 뚫는 관통형 대미지
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_penetrate">_penetrate가 true일 경우에는 쉴드를 뚫는 관통형 대미지 </param>
    public void ApplyDamage(int _damage, bool _penetrate = false)
    {
        //관통형 대미지인경우
        if (_penetrate)
        {
            CurHealth -= _damage;
        }
        //비관통형 대미지인경우 쉴드 먼저 까임
        else
        {
            shield -= _damage;
            if(shield < 0)
            {
                curHealth += shield;
                shield = 0;
            }
        }
    }

    /// <summary>
    /// 방어도를 0으로 초기화
    /// </summary>
    public void ResetShield()
    {
        shield = 0;
    }

    void Heal(int _healamount)
    {
        CurHealth += _healamount;
        if (CurHealth > maxHealth)
        {
            CurHealth = maxHealth;
        }
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
        set { curHealth = value; }
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