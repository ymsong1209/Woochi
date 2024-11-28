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
        List<int> charmIDs = DataCloud.playerData.battleData.charms;

        for (int i = 0; i < charmIcons.Count; ++i)
        {
            if(i < charmIDs.Count)
            {
                BaseCharm charm = GameManager.GetInstance.Library.GetCharm(charmIDs[i]);
                charmIcons[i].SetCharm(charm, IsCharmSetAvailable(charm));
            }
            else
            {
                charmIcons[i].SetCharm(null);
            }
        }
    }

    private bool IsCharmSetAvailable(BaseCharm charm)
    {
        if (!mainCharacter) return false;
        if (!IsCharmReceiverAvailable(charm)) return false;
        return true;
    }

    private bool IsCharmReceiverAvailable(BaseCharm charm)
    {
        for(int i = 0; i <charm.CharmRadius.Length; ++i)
        {
            if(charm.CharmRadius[i] && BattleManager.GetInstance.IsCharacterThere(i))
            {
                return true;
            }
        }
        return false;
    }
    
    private void CharmButtonClicked(BaseCharm selectedCharm)
    {
        if (selectedCharm == null)
            return;
        
        mainCharacter.CharmSkill.SetSkillForCharm(selectedCharm);
        BattleManager.GetInstance.SkillSelected(mainCharacter.CharmSkill);
    }

    private void SetCharmTooltip(BaseCharm charm, Transform transform)
    {
        skillDescriptionUI.Activate(charm);
        
        RectTransform targetRt = transform as RectTransform;
        RectTransform tooltipRt = skillDescriptionUI.transform as RectTransform;
        Vector2 offset = new Vector2(0, targetRt.rect.height);
        UIManager.GetInstance.SetTooltipPosition(targetRt, tooltipRt, offset);
    }
}
