using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTriggerArea : MonoBehaviour
{

    void Start()
    {
        
    }

    void Update()
    {
        // ���콺 ���� ��ư�� Ŭ���Ǿ����� Ȯ��
        if (Input.GetMouseButtonDown(0))
        {
            CheckMouseRaycast(true);
        }
        // ���콺�� ���� ��ü ���� �ִ��� Ȯ�� (�� �����Ӹ���)
        else
        {
            CheckMouseRaycast(false);
        }
    }

    private void CheckMouseRaycast(bool isClick)
    {
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 2D ����ĳ��Ʈ�� �߻��Ͽ� ����� RaycastHit2D ��ü�� ����
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // ����ĳ��Ʈ�� � Collider2D�� �浹�ߴ��� Ȯ��
        if (hit.collider != null)
        {
            // �浹�� ��ü�� �� ��ũ��Ʈ�� ������ GameObject���� Ȯ��
            if (hit.collider.gameObject == gameObject)
            {
                if (isClick)
                {
                    Debug.Log(this.name.ToString());
                    BaseSkill BindedSkill = BattleManager.GetInstance.CurrentSelectedSkill;
                    BattleManager.GetInstance.ExecuteSelectedSkill();
                  
                }
                else
                {
                    //Debug.Log("Mouse is over the GameObject.");
                    // ���⿡ ���콺 ���� ���� ������ �߰�
                }
            }
        }
    }

    void ApplySkill(BaseSkill _skill, BaseCharacter _opponent)
    {
        GameObject CasterGameObject = BattleManager.GetInstance.currentCharacterGameObject;
        BaseCharacter caster = CasterGameObject.GetComponent<BaseCharacter>();
        _skill.ApplySkill(_opponent);
    }

}
