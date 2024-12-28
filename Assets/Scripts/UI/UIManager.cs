using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [HeaderTooltip("Canvas", "UI Canvas")] 
    [SerializeField] private Canvas canvas;
    private RectTransform canvasRt;
    
    [HeaderTooltip("Character Tooltip", "캐릭터에 마우스 올릴 시 적 정보 뜨는 툴팁")]
    public GameObject characterTooltip;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI evasionTxt;
    [SerializeField] private TextMeshProUGUI speedTxt;
    [Space]
    [SerializeField] private BuffPopupUI buffPopupUI;

    [HeaderTooltip("Woochi", "우치 전용 UI")]
    public SorceryGuageUI sorceryGuageUI;
    [SerializeField] private WoochiActionList woochiActionList;

    [HeaderTooltip("Battle Result", "전투 결과")]
    public LevelUpUI levelUpUI;
    
    public RecruitUI recruitUI; 
    
    public EnemySkillNamePopup enemySkillNamePopup;
    
    private void Start()
    {
        BattleManager.GetInstance.OnFocusStart += DeactivateBuffPopUp;
        canvasRt = canvas.GetComponent<RectTransform>();
    }
    
    public void SetCharacterToolTip(BaseCharacter _character)
    {
        characterTooltip.SetActive(true);
        nameTxt.text = $"[{_character.Name}]";
        
        Stat finalStat = _character.FinalStat;
        hpTxt.text = $"체력 : {_character.Health.CurHealth} / {_character.Health.MaxHealth}";;
        evasionTxt.text = $"회피 : {finalStat.GetValue(StatType.Evasion)}";
        speedTxt.text = $"속도 : {finalStat.GetValue(StatType.Speed)}";
    }

    public void SetTooltipPosition(RectTransform targetRt, RectTransform tooltipRt, Vector2 offset)
    {
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(null, targetRt.position);

        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRt, screenPosition,
            null, out localPosition);
        
        tooltipRt.localPosition = localPosition + offset;
    }

    public void DeactivePopup()
    {
        characterTooltip.SetActive(false);
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
