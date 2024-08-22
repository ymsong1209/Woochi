using TMPro;
using UnityEngine;

public class LevelUpUI : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] private GameObject statUI;
    [SerializeField] private TextMeshProUGUI[] hpTxt;
    [SerializeField] private TextMeshProUGUI[] damageTxt;
    [SerializeField] private TextMeshProUGUI[] critTxt;
    [SerializeField] private TextMeshProUGUI[] defTxt;
    [SerializeField] private TextMeshProUGUI[] accTxt;
    [SerializeField] private TextMeshProUGUI[] evasionTxt;
    [SerializeField] private TextMeshProUGUI[] speedTxt;
    [SerializeField] private TextMeshProUGUI[] resistTxt;

    [Header("Skill")]
    [SerializeField] private GameObject skillUI;
    [SerializeField] private TextMeshProUGUI skillTxt;

    public void Show(BaseCharacter character)
    {
        gameObject.SetActive(true);
        statUI.SetActive(true);     skillUI.SetActive(false);
        SetStat(character);
        SetSkill(character);
    }

    private void SetStat(BaseCharacter character)
    {
        hpTxt[0].text = character.Health.MaxHealth.ToString();
        hpTxt[1].text = $"+{character.levelUpStat.maxHealth}";

        damageTxt[0].text = $"{character.FinalStat.minStat} - {character.FinalStat.maxStat}";
        damageTxt[1].text = $"+{character.levelUpStat.maxStat}";

        critTxt[0].text = character.FinalStat.crit.ToString();
        critTxt[1].text = $"+{character.levelUpStat.crit}";

        defTxt[0].text = character.FinalStat.defense.ToString();
        defTxt[1].text = $"+{character.levelUpStat.defense}";

        accTxt[0].text = character.FinalStat.accuracy.ToString();
        accTxt[1].text = $"+{character.levelUpStat.accuracy}";

        evasionTxt[0].text = character.FinalStat.evasion.ToString();
        evasionTxt[1].text = $"+{character.levelUpStat.evasion}";

        speedTxt[0].text = character.FinalStat.speed.ToString();
        speedTxt[1].text = $"+{character.levelUpStat.speed}";

        resistTxt[0].text = character.FinalStat.resist.ToString();
        resistTxt[1].text = $"+{character.levelUpStat.resist}";
    }

    /// <summary>
    /// 스킬 해금 시 다음 함수로 텍스트 설정
    /// </summary>
    private void SetSkill(BaseCharacter character)
    {

    }
}
