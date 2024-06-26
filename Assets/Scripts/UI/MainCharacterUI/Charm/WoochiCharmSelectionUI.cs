using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class WoochiCharmSelectionUI : MonoBehaviour
{
   
    [SerializeField] private List<CharmIcon> charmIcons = new List<CharmIcon>(5);
    private MainCharacter mainCharacter;

    public void Start()
    {
        for (int i = 0; i < charmIcons.Count; i++)
        {
            int index = i;
            Button btn = charmIcons[i].btn;
            btn.onClick.AddListener(() => CharmButtonClicked(charmIcons[index].Charm));
        }
    }

    public void Initialize(bool isEnable)
    {
        gameObject.SetActive(false);
    }
    
    public void Activate()
    {
        gameObject.SetActive(true);
        
        mainCharacter = BattleManager.GetInstance.currentCharacter as MainCharacter;
        if (!mainCharacter && !mainCharacter.IsMainCharacter)
        {
            Debug.LogError("우치 차례가 아님");
            return;
        }
        
        SetCharmOnButton();
    }
    
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void SetCharmOnButton()
    {
        //모든 부적 비활성화
        DisableCharms();
        
        BaseCharm[] charmList = GameManager.GetInstance.CharmList;
        for (int i = 0; i < charmList.Length; ++i)
        {
            if (charmList[i])
            {
                BaseCharm charm = charmList[i];
                charmIcons[i].SetCharm(charm, IsCharmSetAvailable(charm));
            }
        }
    }
    private bool IsCharmSetAvailable(BaseCharm charm)
    {
        if (!mainCharacter) return false;
        if (!IsCharmReceiverAvailable(charm)) return false;
        return true;
    }

    bool IsCharmReceiverAvailable(BaseCharm charm)
    {
        for(int i = 0;i<charm.CharmRadius.Length;++i)
        {
            if(charm.CharmRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
            {
                return true;
            }
        }
        return false;
    }
    
    private void DisableCharms()
    {
        foreach (CharmIcon icon in charmIcons)
        {
            icon.SetCharm(null);
        }
    }
    
    public void CharmButtonClicked(BaseCharm _charm)
    {
        if (_charm == null)
            return;
        
        mainCharacter.CharmSkill.Charm = _charm;
        mainCharacter.CharmSkill.SetSkillForCharm();
        BattleManager.GetInstance.SkillSelected(mainCharacter.CharmSkill);
    }
}
