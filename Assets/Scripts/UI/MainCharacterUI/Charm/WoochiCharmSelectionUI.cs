using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class WoochiCharmSelectionUI : MonoBehaviour
{
    [SerializeField] private SkillDescriptionUI skillDescriptionUI;
    [SerializeField] private List<CharmIcon> charmIcons = new List<CharmIcon>(5);
    private MainCharacter mainCharacter;

    public void Start()
    {
        for (int i = 0; i < charmIcons.Count; i++)
        {
            int index = i;
            Button btn = charmIcons[i].btn;
            btn.onClick.AddListener(() => CharmButtonClicked(charmIcons[index].Charm));
            charmIcons[i].OnShowTooltip += SetCharmTooltip;
            charmIcons[i].OnHideTooltip += () => skillDescriptionUI.gameObject.SetActive(false);
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

        List<int> charmIDs = DataCloud.playerData.battleData.charms;

        for(int i = 0; i < charmIDs.Count; ++i)
        {
            BaseCharm charm = GameManager.GetInstance.Library.GetCharm(charmIDs[i]);
            charmIcons[i].SetCharm(charm, IsCharmSetAvailable(charm));
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

    private void SetCharmTooltip(BaseCharm charm, Transform transform)
    {
        skillDescriptionUI.Activate(charm);
        skillDescriptionUI.transform.position = transform.position + new Vector3(0, 100, 0);
    }
}
