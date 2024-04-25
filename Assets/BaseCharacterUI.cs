using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterUI : MonoBehaviour
{
    public SpriteRenderer hpBar;

    public void UpdateHPBar(float _curHealth, float _maxHealth)
    {
        hpBar.transform.localScale = new Vector3(_curHealth / _maxHealth, 1, 1);
    }
}
