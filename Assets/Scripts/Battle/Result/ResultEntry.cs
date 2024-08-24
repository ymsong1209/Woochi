using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI hpTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private TextMeshProUGUI expTxt;
    [SerializeField] private Button levelUp;

    private BaseCharacter owner;

    private void Start()
    {
        levelUp.onClick.AddListener(ShowLevelUp);
    }

    public void Set(BaseCharacter character)
    {
        if(character == null)
        {
            gameObject.SetActive(false);
            return;
        }

        owner = character;
        gameObject.SetActive(true);
        nameTxt.text = owner.Name;
        ChangeExp();
        hpTxt.text = $"{owner.Health.CurHealth}/{owner.Health.MaxHealth}";
    }

    private void ChangeExp()
    {
        string[] rankNames = owner.IsMainCharacter ? DataCloud.woochiRankNames : DataCloud.allyRankNames;
        int plusExp = owner.level.plusExp;

        bool isLevelUp = owner.level.AddExp();
        if(isLevelUp)
            levelUp.gameObject.SetActive(true);
        else
            levelUp.gameObject.SetActive(false);

        rankTxt.text = rankNames[owner.level.rank - 1];
        expTxt.text = $"{owner.level.exp}/{owner.level.GetRequireExp()}(+{plusExp})";
    }

    private void ShowLevelUp()
    {
        if (owner == null) return;

        UIManager.GetInstance.levelUpUI.Show(owner);
    }
}
