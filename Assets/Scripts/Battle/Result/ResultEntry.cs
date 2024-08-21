using TMPro;
using UnityEngine;

public class ResultEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private TextMeshProUGUI expTxt;

    public void Set(BaseCharacter character)
    {
        if(character == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        nameTxt.text = character.Name;
        hpTxt.text = $"{character.Health.CurHealth}/{character.Health.MaxHealth}";
        ChangeExp(character);
    }

    private void ChangeExp(BaseCharacter character)
    {
        string[] rankNames = character.IsMainCharacter ? DataCloud.woochiRankNames : DataCloud.allyRankNames;
        int plusExp = character.level.plusExp;

        character.level.AddExp();
        rankTxt.text = rankNames[character.level.rank - 1];
        expTxt.text = $"{character.level.exp}/{character.level.GetRequireExp()}(+{plusExp})";
    }
}
