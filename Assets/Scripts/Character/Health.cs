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
    /// ������� �޴� ����, _penetrate�� true�� ��쿡�� ���带 �մ� ������ �����
    /// </summary>
    /// <param name="_damage"></param>
    /// <param name="_penetrate">_penetrate�� true�� ��쿡�� ���带 �մ� ������ ����� </param>
    public void ApplyDamage(int _damage, bool _penetrate = false)
    {
        //������ ������ΰ��
        if (_penetrate)
        {
            CurHealth -= _damage;
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
    }

    /// <summary>
    /// ���� 0���� �ʱ�ȭ
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
            //������ ������� ü���� 0�� �ɼ� ����
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