using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTriggerArea : MonoBehaviour
{
    

    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }


    void ApplySkill(BaseSkill _skill, BaseCharacter _opponent)
    {
        GameObject CasterGameObject = BattleManager.GetInstance.currentCharacterGameObject;
        BaseCharacter caster = CasterGameObject.GetComponent<BaseCharacter>();
        _skill.ApplySkill(_opponent);
    }

}
