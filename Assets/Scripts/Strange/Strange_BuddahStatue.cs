using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Strange_BuddahStatue : BaseStrange
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button investigateBtn;
    [SerializeField] private Button destroyBtn;
    [SerializeField] private Button ignoreBtn;
    [SerializeField] private Image statueImage;
    [SerializeField] private Image investigatePositiveImage;
    [SerializeField] private Image investigateNegativeImage;
    [SerializeField] private Image destroyPositiveImage;
    [SerializeField] private Image destroyNegativeImage;
    [SerializeField] private Image ignoreImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI buffdescriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
        continueBtn.onClick.AddListener(() => Deactivate());
        investigateBtn.onClick.AddListener(() => Investigate());
        destroyBtn.onClick.AddListener(() => DestroyStatue());
        ignoreBtn.onClick.AddListener(() => Ignore());
    }

    public override void Initialize()
    {
        base.Initialize();
        
        continueBtn.gameObject.SetActive(false);
        investigateBtn.gameObject.SetActive(true);
        destroyBtn.gameObject.SetActive(true);
        ignoreBtn.gameObject.SetActive(true);
        
        statueImage.gameObject.SetActive(true);
        investigatePositiveImage.gameObject.SetActive(false);
        investigateNegativeImage.gameObject.SetActive(false);
        destroyPositiveImage.gameObject.SetActive(false);
        destroyNegativeImage.gameObject.SetActive(false);
        ignoreImage.gameObject.SetActive(false);
        
        descriptionText.text = "외진 곳의 불상을 보았다!\n" +
                               "관리가 되어 있지 않아 지저분하다.\n" +
                               "나는...";
        buffdescriptionText.gameObject.SetActive(false);
    }
    
    private void Investigate()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            investigatePositiveImage.gameObject.SetActive(true);
            descriptionText.text = "오래된 영험한 불상입니다!\n" +
                                   "당신에게 축복을 내립니다.";
            buffdescriptionText.gameObject.SetActive(true);
            buffdescriptionText.text = "다음 3번의 전투에서\n" +
                                       "모든 아군 속도 +10 회피 +5.";
            foreach(BaseCharacter character in BattleManager.GetInstance.Allies.AllCharacter)
            {
                GameObject statGameObject = new GameObject("BuddahStatBuff");
                statGameObject.transform.SetParent(character.transform);
                StatBuff buff = statGameObject.AddComponent<StatBuff>();
                buff.BuffName = "불상의 축복";
                buff.BuffDurationTurns = -1;
                buff.BuffBattleDurationTurns = 3;
                buff.changeStat.speed = 10;
                buff.changeStat.evasion = 5;
                buff.IsRemovableDuringBattle = false;
                buff.IsRemoveWhenBattleEnd = false;
                character.ApplyBuff(character, character, buff); 
            }
        }
        else
        {
            investigateNegativeImage.gameObject.SetActive(true);
            descriptionText.text = "귀불입니다!\n" +
                                   "당신에게 저주를 내립니다.";
            buffdescriptionText.gameObject.SetActive(true);
            buffdescriptionText.text = "다음 3번의 전투에서\n" +
                                       "모든 아군 피해 -3 명중 -10.";
            foreach(BaseCharacter character in BattleManager.GetInstance.Allies.AllCharacter)
            {
                GameObject statGameObject = new GameObject("BuddahStatDeBuff");
                statGameObject.transform.SetParent(character.transform);
                StatDeBuff buff = statGameObject.AddComponent<StatDeBuff>();
                buff.BuffName = "귀불의 저주";
                buff.BuffDurationTurns = -1;
                buff.BuffBattleDurationTurns = 3;
                buff.changeStat.minStat = -3;
                buff.changeStat.maxStat = -3;
                buff.changeStat.evasion = -10;
                buff.IsRemovableDuringBattle = false;
                buff.IsRemoveWhenBattleEnd = false;
                character.ApplyBuff(character, character, buff); 
            }
        }
        ContinueEvent();
    }

    private void DestroyStatue()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            destroyPositiveImage.gameObject.SetActive(true);
            descriptionText.text = "귀불을 파괴했습니다!\n" +
                                   "주위 요기가 약해지는 것이 느껴집니다!";
            buffdescriptionText.gameObject.SetActive(true);
            buffdescriptionText.text = "다음 3번의 전투에서\n" +
                                       "모든 아군의 스탯 +7.";
            foreach(BaseCharacter character in BattleManager.GetInstance.Allies.AllCharacter)
            {
                GameObject statGameObject = new GameObject("BuddahStatBuff");
                statGameObject.transform.SetParent(character.transform);
                StatBuff buff = statGameObject.AddComponent<StatBuff>();
                buff.BuffName = "파괴된 귀불의 축복";
                buff.BuffDurationTurns = -1;
                buff.BuffBattleDurationTurns = 3;
                buff.changeStat.minStat = 7;
                buff.changeStat.maxStat = 7;
                buff.changeStat.speed = 7;
                buff.changeStat.evasion = 7;
                buff.changeStat.accuracy = 7;
                buff.changeStat.crit = 7;
                buff.changeStat.resist = 7;
                buff.changeStat.defense = 7;
                buff.IsRemovableDuringBattle = false;
                buff.IsRemoveWhenBattleEnd = false;
                character.ApplyBuff(character, character, buff); 
            }
        }
        else
        {
            destroyNegativeImage.gameObject.SetActive(true);
            descriptionText.text = "오래된 영험한 불상을 파괴했습니다!\n" +
                                   "몸이 무거워집니다...";
            buffdescriptionText.gameObject.SetActive(true);
            buffdescriptionText.text = "다음 3번의 전투에서\n" +
                                       "모든 아군 속도 -10 회피 -10.";
            foreach(BaseCharacter character in BattleManager.GetInstance.Allies.AllCharacter)
            {
                GameObject statGameObject = new GameObject("BuddahStatDeBuff");
                statGameObject.transform.SetParent(character.transform);
                StatDeBuff buff = statGameObject.AddComponent<StatDeBuff>();
                buff.BuffName = "파괴된 불상의 저주";
                buff.BuffDurationTurns = -1;
                buff.BuffBattleDurationTurns = 3;
                buff.changeStat.evasion = -10;
                buff.changeStat.speed = -10;
                buff.IsRemovableDuringBattle = false;
                buff.IsRemoveWhenBattleEnd = false;
                character.ApplyBuff(character, character, buff); 
            }
        }
        ContinueEvent();
    }

    private void Ignore()
    {
        ignoreImage.gameObject.SetActive(true);
        descriptionText.text = "아무런 효과가 없습니다\n" +
                               "아직까지는요...";
        ContinueEvent();
    }

    private void ContinueEvent()
    {
        continueBtn.gameObject.SetActive(true);
        investigateBtn.gameObject.SetActive(false);
        destroyBtn.gameObject.SetActive(false);
        ignoreBtn.gameObject.SetActive(false);
    }
}
