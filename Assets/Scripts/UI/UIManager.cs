using UnityEngine;
using TMPro;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [HeaderTooltip("HP tooltip", "체력바 툴팁")]
    [SerializeField] GameObject hpTooltip;
    [SerializeField] TextMeshProUGUI hpTooltipText;

    [HeaderTooltip("Enemy Tooltip", "적 캐릭터에 마우스 올릴 시 적 정보 뜨는 툴팁")]
    public GameObject enemyTooltip;
    [SerializeField] private TextMeshProUGUI enemyNameTxt;
    [SerializeField] private TextMeshProUGUI enemyEvasionTxt;
    [SerializeField] private TextMeshProUGUI enemySpeedTxt;
    [Space]
    [SerializeField] private AllyCharacterUI allyCharacterUI;
    [SerializeField] private BuffPopupUI buffPopupUI;

    [HeaderTooltip("Woochi", "우치 전용 UI")]
    public SorceryGuageUI sorceryGuageUI;
    [SerializeField] private WoochiActionList woochiActionList;

    [HeaderTooltip("Battle Result", "전투 결과")]
    public LevelUpUI levelUpUI;
    
    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += DeactivateBuffPopUp;
    }
    
    public void SetHPTooltip(bool isActivate, HPBar hpBar = null)
    {
        hpTooltip.SetActive(isActivate);

        if(hpBar)
        {
            hpTooltip.transform.position = hpBar.GetPosition();
            hpTooltipText.text = hpBar.GetTooltipText();
        }
    }

    public void SetEnemyToolTip(BaseCharacter _character)
    {
        enemyTooltip.SetActive(true);
        enemyNameTxt.text = _character.Name;
        
        Stat finalStat = _character.FinalStat;
        enemyEvasionTxt.text = $"회피 : {finalStat.GetValue(StatType.Evasion)}";
        enemySpeedTxt.text = $"속도 : {finalStat.GetValue(StatType.Speed)}";
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

    public void DeactivePopup()
    {
        enemyTooltip.SetActive(false);
        buffPopupUI.Deactivate();
    }
    
    #region BuffPopupUI
    public BuffPopupUI BuffPopupUI => buffPopupUI;
    public void ActivateBuffPopUp(BuffIcon icon)
    {
        buffPopupUI.Activate(icon);
    }
    
    public void DeactivateBuffPopUp()
    {
        buffPopupUI.Deactivate();
    }
    #endregion
}
