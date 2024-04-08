using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    public GameObject skillTooltip;
    [SerializeField] private TextMeshProUGUI skillTooltipTxt;

    void Start()
    {
        
    }

    public void SetSkillToolTip(BaseSkill _skill, Vector3 position)
    {
        skillTooltip.SetActive(true);
        skillTooltip.transform.position = position;
        skillTooltipTxt.text = _skill.Name;
    }
}
