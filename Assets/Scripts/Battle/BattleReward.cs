using System.Collections.Generic;
using UnityEngine;
using DataTable;
using UnityEngine.UI;
using TMPro;

public class BattleReward : MonoBehaviour
{
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI goldTxt;

    [Header("Reward")]
    [SerializeField, ReadOnly(true)] private RandomList<RareType> rarityList;
    private HashSet<Reward> rewardSet = new HashSet<Reward>();
    [SerializeField] private List<RewardUI> rewardsList;
    [SerializeField] private int rewardCount = 5;

    [Header("Reroll")]
    [SerializeField] private Button rerollBtn;
    [SerializeField] private TextMeshProUGUI rerollPriceTxt;
    [SerializeField] private int rerollPrice = 100;

    [Header("Exp")]
    [SerializeField] private int[] normalExps;
    [SerializeField] private int[] eliteExps;

    [Header("Result")]
    [SerializeField] private BattleResultUI resultUI;

    private int grade = 0;      // 역경 단계

    void Start()
    {
        nextBtn.onClick.AddListener(Next);
        rerollBtn.onClick.AddListener(ReRoll);
        EventManager.GetInstance.onChangedGold += SetGold;
        EventManager.GetInstance.onSelectReward += SetInteractable;

        rewardPanel.SetActive(false);
    }

    public void SetReward(BattleResult result)
    {
        Init();
        grade = result.hardShipGrade;
        GetExpReward(result.isElite);
        SetReward();
    }

    private void Init()
    {
        SetInteractable(true);
        SetReroll(100);
        SetGold();
    }

    /// <summary>
    /// 역경 등급에 따른 확률로 희귀도 구하기
    /// </summary>
    private RareType GetRarity(int grade)
    {
        var data = RarityData.GetDictionary()[grade];
        var probabilityList = new List<float>() { data.Lowest, data.Lower, data.Middle, data.Higher, data.Highest };
        
        for(int i = 0; i < rarityList.list.Count; i++)
        {
            rarityList.list[i].probability = probabilityList[i];
        }

        return rarityList.Get();
    }

    private void GetExpReward(bool isElite)
    {
        AllyFormation allyFormation = BattleManager.GetInstance.Allies;

        foreach (var character in allyFormation.AllCharacter)
        {
            if(character.IsDead) continue;

            if(isElite)
                character.level.plusExp += eliteExps[grade];
            else
                character.level.plusExp += normalExps[grade];
        }

        GameManager.GetInstance.SaveData();
    }

    private void SetReward()
    {
        rewardSet.Clear();

        while(rewardSet.Count < rewardCount)
        {
            RareType rarity = GetRarity(grade);
            rewardSet.Add(GameManager.GetInstance.Library.GetReward(rarity));
        }

        SetRewardUI();
    }

    private void SetRewardUI()
    {
        var list = new List<Reward>(rewardSet);

        for(int i = 0; i < rewardsList.Count; i++)
        {
            rewardsList[i].Initialize(list[i]);
        }
    }

    private void ReRoll()
    {
        if (HelperUtilities.Buy(rerollPrice))
        {
            SetReward();
            SetReroll(rerollPrice * 2);
        }
    }

    /// <summary>
    /// 화살표 눌렀을 때 보상 창 닫기
    /// 보상 받았으면 저장
    /// </summary>
    private void Next()
    {
        rewardPanel.SetActive(false);
        resultUI.AfterGetReward();
        GameManager.GetInstance.SaveData();
    }

    private void SetGold()
    {
        goldTxt.text = $"{DataCloud.playerData.gold} 개";
    }

    /// <summary>
    /// 보상 UI 상호작용 설정
    /// active가 true면 보상 선택 가능, 다음 버튼 선택 불가
    /// </summary>
    /// <param name="active"></param>
    private void SetInteractable(bool active)
    {
        foreach(var rewardUI in rewardsList)
        {
            rewardUI.SetInteractable(active);
        }

        nextBtn.interactable = !active;
        rerollBtn.interactable = active;
    }

    /// <summary>
    /// 리롤 비용 + 리롤 비용 텍스트 설정
    /// </summary>
    private void SetReroll(int newPrice)
    {
        rerollPrice = newPrice;
        rerollPriceTxt.text = rerollPrice.ToString();
    }
}
