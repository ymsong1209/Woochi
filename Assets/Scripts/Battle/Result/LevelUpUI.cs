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
    }

    private void SetSkill(BaseCharacter character)
    {

    }
}
