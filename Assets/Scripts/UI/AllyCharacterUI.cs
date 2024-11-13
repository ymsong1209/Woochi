using TMPro;
using UnityEngine;

/// <summary>
/// 아군 캐릭터 정보와 관련된 UI들을 관리하는 클래스
/// 캐릭터 초상화, 이름, 스탯 등을 표시
/// </summary>
public class AllyCharacterUI : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private TextMeshProUGUI characterName;

    [Header("Stat")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private AllyStat[] stats;
    
    void Start()
    {
        #region 이벤트 등록
        BattleManager.GetInstance.ShowCharacterUI += ShowCharacterUI;
        #endregion
    }

    public void ShowCharacterUI(BaseCharacter _character, bool isEnable = true)
    {
        if(!_character.IsAlly)
            return;

        ShowCharacterInfo(_character);
        ShowCharacterStat(_character);
    }

    /// <summary>
    /// 캐릭터 정보를 조작 UI에 표시
    /// </summary>
    /// <param name="_character"></param>
    private void ShowCharacterInfo(BaseCharacter _character)
    {
        characterName.text = _character.Name;
    }

    /// <summary>
    /// 캐릭터 스탯을 UI에 표시
    /// </summary>
    /// <param name="_character"></param>
    private void ShowCharacterStat(BaseCharacter _character)
    {
        Stat stat = _character.FinalStat;
        Stat buffStat = _character.BuffStat;
        hpText.text = $"{_character.Health.CurHealth} / {_character.Health.MaxHealth}";
        
        for(int i = 1; i < stats.Length; i++)
        {
            if ((StatType)i == StatType.MinDamage)
            {
                float min = stat.GetValue(StatType.MinDamage);
                float max = stat.GetValue(StatType.MaxDamage);
                stats[i].SetDamageText(min, max);
            }
            else
            {
                float value = stat.GetValue((StatType)i);
                float changeValue = buffStat.GetValue((StatType)i);
                stats[i].SetText(value, changeValue);
            }
        }
    }

}
