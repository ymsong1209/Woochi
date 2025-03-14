using System.Collections.Generic;
using UnityEngine;
using DataTable;
using UnityEngine.UI;
using TMPro;

public class BattleReward : MonoBehaviour
{
    [Header("Tool")]
    [SerializeField] private List<RewardUI> toolList;

    [Header("Reward")]
    [SerializeField, ReadOnly(true)] private RandomList<RareType> rarityList;
    [SerializeField] private List<RewardUI> rewardsList;
    private HashSet<Reward> rewardSet = new HashSet<Reward>();

    [Header("Reroll")]
    [SerializeField] private Button rerollBtn;
    [SerializeField] private TextMeshProUGUI rerollPriceTxt;
    [SerializeField] private int rerollPrice = 100;
    
    [Header("Result")]
    [SerializeField] private BattleResultUI resultUI;

    [Header("Object")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private RewardPopup rewardPopup;

    private int grade = 0;      // 역경 단계

    void Start()
    {
        #region Event Register
        nextBtn.onClick.AddListener(Next);
        rerollBtn.onClick.AddListener(ReRoll);
        EventManager.GetInstance.onChangedGold += SetGold;
        EventManager.GetInstance.onSelectReward += SetInteractable;

        for(int i = 0; i < toolList.Count; i++)
        {
            toolList[i].OnUIEvent += ProcessUIEvent;
        }

        for(int i = 0; i < rewardsList.Count; i++)
        {
            rewardsList[i].OnUIEvent += ProcessUIEvent;
        }
        #endregion
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
        SetTool();
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
        int exp = DataCloud.GetExp(grade, isElite);
        
        foreach (var character in allyFormation.AllCharacter)
        {
            if(character.IsDead) continue;
            character.level.plusExp += exp;
        }

        GameManager.GetInstance.SaveData();
    }

    private void SetReward()
    {
        rewardSet.Clear();

        int rewardCount = rewardsList.Count;
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

    private void SetTool()
    {
        foreach(var toolUI in toolList)
        {
            toolUI.Initialize();
        }
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
    
    private void ProcessUIEvent(RewardUI rewardUI, UIEvent uiEvent)
    {
        switch(uiEvent)
        {
            case UIEvent.MouseEnter:
                rewardPopup.ShowTooltip(rewardUI);
                break;
            case UIEvent.MouseExit:
                rewardPopup.HideInfo();
                break;
            case UIEvent.MouseClick:
                rewardPopup.ShowResult(rewardUI.GetPopupText());
                break;
        }
    }
}
