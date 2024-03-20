using UnityEngine;

public class PlayableSkill : BaseSkill
{

    [SerializeField, ReadOnly] PlayableCharacter character;


    /// <summary>
    /// 스킬 아이콘에 스킬을 마우스로 선택, 혹은 키보드의 1234버튼으로 스킬을 선택 한 경우 스킬을 활성화시키는 함수
    /// 스킬이 활성화가 되면 스킬의 범위가 표시된다.
    /// </summary>
    public override void SetSelect(bool _selected)
    {
        //스킬을 선택한 경우
        if (_selected)
        {
            //기존에 활성화된 스킬이 있을 경우, 해당 스킬은 비활성화되어야함.
            if (character.IsSkillSelected)
            {

            }
            // 이후 
        }
        //스킬 선택 해제
        else
        {

        }

        character.IsSkillSelected = _selected;

    }

    /// <summary>
    /// 
    /// </summary>
    public override void ActivateSkill()
    {
        //스킬 테두리 icon이 빛나야함

        //skillSO

    }

    #region Validation
    private void OnValidate()
    {

    }
    #endregion
}