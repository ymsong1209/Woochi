using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [HeaderTooltip("Skill Tooltip","스킬 아이콘에 마우스 올릴 시 뜨는 툴팁")]
    public GameObject skillTooltip;
    [SerializeField] private TextMeshProUGUI skillNameTxt;

    [HeaderTooltip("Enemy Tooltip", "적 캐릭터에 마우스 올릴 시 적 정보 뜨는 툴팁")]
    public GameObject enemyTooltip;
    [SerializeField] private TextMeshProUGUI enemyNameTxt;
    [SerializeField] private TextMeshProUGUI enemyHPTxt;
    [SerializeField] private TextMeshProUGUI enemyEvasionTxt;
    [SerializeField] private TextMeshProUGUI enemySpeedTxt;

    [SerializeField] private AllyCharacterUI allyCharacterUI;
    [SerializeField] private BuffPopupUI buffPopupUI;
    
    [SerializeField] private WoochiActionList woochiActionList;
    [SerializeField] private Image sorceryPoint;
    [SerializeField] private Image sorceryPointBackground;
    [SerializeField] private GameObject charmToolTip;
    
    public void SetSkillToolTip(BaseSkill _skill, Vector3 position)
    {
        skillTooltip.SetActive(true);
        skillTooltip.transform.position = position;
        skillNameTxt.text = _skill.Name;
    }

    public void SetEnemyToolTip(BaseCharacter _character)
    {
        enemyTooltip.SetActive(true);
        enemyNameTxt.text = _character.characterStat.CharacterName;
        enemyHPTxt.text = $"체력 : {_character.Health.CurHealth} / {_character.Health.MaxHealth}";
        enemyEvasionTxt.text = $"회피 : {_character.Evasion}";
        enemySpeedTxt.text = $"속도 : {_character.Speed}";
    }

    public void OnCharacterDamaged(BaseCharacter _character)
    {
        if (_character.IsAlly)
        {

        }
        else
        {
            SetEnemyToolTip(_character);
        }
    }
    
    #region BuffPopupUI
    public BuffPopupUI BuffPopupUI => buffPopupUI;
    public void ActivateBuffPopUp(Vector3 Mouseposition)
    {
        buffPopupUI.Activate(Mouseposition);
    }
    
    public void DeactivateBuffPopUp()
    {
        buffPopupUI.Deactivate();
    }
    #endregion

    #region MainCharacterUI
    public void ResetWoochiActionList()
    {
        woochiActionList.Reset();
    }

    public void SetSorceryPointUI(int point)
    {
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            Debug.LogError("MainCharacter is null");
            return;
        }
        float scale = (float)point / mainCharacter.MaxSorceryPoints;
        sorceryPoint.DOFillAmount(scale, 1f).SetEase(Ease.OutCubic);
    }
    
    public void SetSorceryPointBackgroundUI(int point)
    {
        MainCharacter mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter)
        {
            Debug.LogError("MainCharacter is null");
            return;
        }
        float scale = (float)point / mainCharacter.MaxSorceryPoints;
        sorceryPointBackground.DOFillAmount(scale, 1f).SetEase(Ease.OutCubic);
    }
    
    public void SetCharmToolTip(BaseCharm _charm, Vector3 position)
    {
        skillTooltip.SetActive(true);
        skillTooltip.transform.position = position;
        skillNameTxt.text = _charm.CharmName;
    }
    
    #endregion MainCharacterUI
}
