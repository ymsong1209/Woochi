using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 기연의 Id, gameobject를 library에다가 입력후,
/// 이번 스테이지에서 출현하고 싶은 기연을 MapConfig에다 입력해주면됨.
/// 기연이 나올 확률도 MapConfig에서 조절
/// </summary>
public class StrangeManager : SingletonMonobehaviour<StrangeManager>
{
    public GameObject StrangeParent;
    
    private List<BaseStrange> luckyStranges = new List<BaseStrange>();
    private List<BaseStrange> unKnownStranges = new List<BaseStrange>();
    private List<BaseStrange> unLuckyStranges = new List<BaseStrange>();
    
    // Start is called before the first frame update
    void Start()
    {
        StrangeParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(MapConfig config)
    {
        foreach(int strangeID in config.StrangeTemplates)
        {
            GameObject strangeObject = Instantiate(GameManager.GetInstance.Library.GetStrange(strangeID),StrangeParent.transform);
            BaseStrange strange = strangeObject.GetComponent<BaseStrange>();
            strange.Initialize();
            
            if(strange.StrangeType == StrangeType.Lucky)
            {
                luckyStranges.Add(strange);
            }
            else if(strange.StrangeType == StrangeType.UnKnown)
            {
                unKnownStranges.Add(strange);
            }
            else if(strange.StrangeType == StrangeType.UnLucky)
            {
                unLuckyStranges.Add(strange);
            }
        }
    }
    public void ActivateStrange(StrangeType type)
    {
        List<BaseStrange> selectedList = GetStrangeListByType(type);

        if (selectedList.Count > 0)
        {
            int randomIndex = Random.Range(0, selectedList.Count);
            selectedList[randomIndex].Activate();
        }
        else
        {
            Debug.LogWarning("No stranges of type " + type + " found to activate.");
        }
    }

    private List<BaseStrange> GetStrangeListByType(StrangeType type)
    {
        switch (type)
        {
            case StrangeType.Lucky:
                return luckyStranges;
            case StrangeType.UnKnown:
                return unKnownStranges;
            case StrangeType.UnLucky:
                return unLuckyStranges;
            default:
                return new List<BaseStrange>();
        }
    }
    
    #region Getter Setter
    
    #endregion Getter Setter
}
