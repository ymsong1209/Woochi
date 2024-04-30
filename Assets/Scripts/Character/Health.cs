using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class HPEvent : UnityEvent<float, float> { }

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float curHealth;
    [SerializeField] private float shield;

    public HPEvent OnHealthChanged;

    /// <summary>
    /// 대미지를 받는 공식, _penetrate가 true일 경우에는 쉴드를 뚫는 관통형 대미지
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_penetrate">_penetrate가 true일 경우에는 쉴드를 뚫는 관통형 대미지 </param>
    public void ApplyDamage(float _damage, bool _penetrate = false)
    {
        BaseCharacter character = gameObject.GetComponent<BaseCharacter>();
        if (character == null) return;

        //관통형 대미지인경우
        if (_penetrate == false)
        {
            curHealth = Mathf.Clamp(curHealth - _damage, 0, maxHealth);
            Debug.Log("Curhealth : " + curHealth);
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

        character.PlayAnimation(AnimationType.Damaged);
        OnHealthChanged.Invoke(curHealth, maxHealth);
    }

    /// <summary>
    /// 방어도를 0으로 초기화
    /// </summary>
    public void ResetShield()
    {
        shield = 0;
    }

    public void Heal(int _healamount)
    {
        curHealth += _healamount;
        curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
        OnHealthChanged.Invoke(curHealth, maxHealth);
    }

    public bool CheckHealthZero()
    {
        if (curHealth <= 0)
        {
            //관통형 대미지로 체력이 0이 될수 있음
            shield = 0;
            return true;
        }
        else return false;
    }



    #region Getter Setter

    public float MaxHealth
    {
        get { return maxHealth; }
        set { 
            maxHealth = value;
        }
    }

    public float CurHealth
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