using System.Collections.Generic;
using UnityEngine;
using DataTable;
using UnityEngine.UI;

public class BattleReward : MonoBehaviour
{
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button nextBtn;


    [SerializeField, ReadOnly(true)] private RandomList<RareType> rarityList;
    private HashSet<Reward> rewardSet = new HashSet<Reward>();

    [SerializeField] private int rewardCount = 5;

    void Start()
    {
        nextBtn.onClick.AddListener(Next);
        rewardPanel.SetActive(false);
    }

    public void ShowReward(int hardShip)
    {
        int grade = CalculateGrade(hardShip);

        SetReward(grade);

        foreach(Reward reward in rewardSet)
        {
            Debug.Log(reward.rewardName);
        }

        DataCloud.playerData.gold += 100;

        rewardPanel.SetActive(true);
    }

    /// <summary>
    /// 역경 수치를 역경 등급으로 변환
    /// </summary>
    private int CalculateGrade(int hardShip)
    {
        int grade = Mathf.Clamp(hardShip - 4, 0, 99);
        return grade;
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

    private void SetReward(int grade)
    {
        while(rewardSet.Count < rewardCount)
        {
            Debug.Log("Reward Set Count: " + rewardSet.Count + " / " + rewardCount);
            RareType rarity = GetRarity(grade);
            rewardSet.Add(GameManager.GetInstance.Library.GetReward(rarity));
        }
    }

    private void Next()
    {
        rewardPanel.SetActive(false);
        MapManager.GetInstance.CompleteNode();
    }
}
