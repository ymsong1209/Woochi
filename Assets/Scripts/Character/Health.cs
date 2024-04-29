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
    /// ������� �޴� ����, _penetrate�� true�� ��쿡�� ���带 �մ� ������ �����
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_penetrate">_penetrate�� true�� ��쿡�� ���带 �մ� ������ ����� </param>
    public void ApplyDamage(float _damage, bool _penetrate = false)
    {
        BaseCharacter character = gameObject.GetComponent<BaseCharacter>();
        if (character == null) return;

        //������ ������ΰ��
        if (_penetrate == false)
        {
            curHealth = Mathf.Clamp(curHealth - _damage, 0, maxHealth);
            Debug.Log("Curhealth : " + curHealth);
        }
        //������� ������ΰ�� ���� ���� ����
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
    /// ���� 0���� �ʱ�ȭ
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
            //������ ������� ü���� 0�� �ɼ� ����
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