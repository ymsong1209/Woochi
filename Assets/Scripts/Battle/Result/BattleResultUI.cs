using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct BattleResult
{
    public int hardShipGrade;   // 역경 단계
    public bool isElite;   // 정예 여부
}

public class BattleResultUI : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI hardShipTxt;
    [SerializeField] private TextMeshProUGUI goldTxt;
    [SerializeField] private Button rewardBtn;
    [SerializeField] private Button finishBtn;

    [Header("Class")]
    [SerializeField] private List<ResultEntry> entries;
    [SerializeField] private BattleReward reward;   // 전투 보상

    void Start()
    {
        panel.SetActive(false);
        rewardBtn.gameObject.SetActive(true);
        finishBtn.gameObject.SetActive(false);
    }

    public void Show(BattleResult result)
    {
        panel.SetActive(true);
        hardShipTxt.text = $"{result.hardShipGrade} 단계";

        reward.SetReward(result);
        SetEntry();
        SetGold();

        MapManager.GetInstance.SaveMap();
    }

    /// <summary>
    /// 보상을 받은 후 한번 더 캐릭터 정보 갱신
    /// </summary>
    public void AfterGetReward()
    {
        SetEntry();
        rewardBtn.gameObject.SetActive(false);
        finishBtn.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 캐릭터 정보 갱신
    /// </summary>
    private void SetEntry()
    {
        var allyFormation = BattleManager.GetInstance.Allies;
        var allies = allyFormation.AllCharacter;
        allies.Sort((a, b) => a.ID.CompareTo(b.ID));

        for(int i = 0; i < entries.Count; i++)
        {
            if(i < allies.Count)
                entries[i].Set(allies[i]);
            else
                entries[i].gameObject.SetActive(false);
        }
    }

    private void SetGold()
    {
        int gold = 100;
        HelperUtilities.AddGold(gold);
        goldTxt.text = $"+ {gold}";
    }
}
