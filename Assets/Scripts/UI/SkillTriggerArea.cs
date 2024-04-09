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
        // 마우스 왼쪽 버튼이 클릭되었는지 확인
        if (Input.GetMouseButtonDown(0))
        {
            CheckMouseRaycast(true);
        }
        // 마우스가 현재 객체 위에 있는지 확인 (매 프레임마다)
        else
        {
            CheckMouseRaycast(false);
        }
    }

    private void CheckMouseRaycast(bool isClick)
    {
        // 마우스 위치를 월드 좌표로 변환
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 2D 레이캐스트를 발사하여 결과를 RaycastHit2D 객체에 저장
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        // 레이캐스트가 어떤 Collider2D와 충돌했는지 확인
        if (hit.collider != null)
        {
            // 충돌한 객체가 이 스크립트가 부착된 GameObject인지 확인
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
                    // 여기에 마우스 오버 시의 로직을 추가
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
